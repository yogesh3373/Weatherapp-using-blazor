let map;
let windyInstance;
let apiKey;
let dotnetHelper;
let marker;
let vectorLayer;
let weatherLayer;
let currentLayer = "wind";

// Initialize windyAPI
const options = {
  key: "9Q5T9NpiQYBKifLZ1UF2iugZ3lKyWBZ4", // Replace with your actual Windy API key
  verbose: false,
};

export async function initMap(helper, mapForecastKey) {
  dotnetHelper = helper;

  // Base map layer
  const baseLayer = new ol.layer.Tile({
    source: new ol.source.OSM(),
  });

  // Weather layer
  weatherLayer = new ol.layer.Tile({
    source: new ol.source.XYZ({
      url: `https://tiles.windy.com/tiles/v9.0/wind/{z}/{x}/{y}.png?key=${mapForecastKey}`,
      crossOrigin: "anonymous",
    }),
    opacity: 0.8,
  });

  // Marker layer
  vectorLayer = new ol.layer.Vector({
    source: new ol.source.Vector(),
    style: new ol.style.Style({
      image: new ol.style.Circle({
        radius: 6,
        fill: new ol.style.Fill({ color: "#3498db" }),
        stroke: new ol.style.Stroke({
          color: "#fff",
          width: 2,
        }),
      }),
    }),
  });

  // Initialize map
  map = new ol.Map({
    target: "map",
    layers: [baseLayer, weatherLayer, vectorLayer],
    view: new ol.View({
      center: ol.proj.fromLonLat([78.9629, 20.5937]),
      zoom: 5,
    }),
  });

  // Initialize Windy
  try {
    await loadWindyAPI();
    initWindyOverlay();
  } catch (error) {
    console.error("Failed to load Windy API:", error);
  }

  // Add click handler
  map.on("click", async function (evt) {
    const coords = ol.proj.transform(evt.coordinate, "EPSG:3857", "EPSG:4326");
    await addMarker(coords);
    await dotnetHelper.invokeMethodAsync("OnMapClick", coords[1], coords[0]);
  });
}

function initWindyOverlay() {
  // Get map container dimensions
  const mapContainer = map.getTargetElement();
  const width = mapContainer.clientWidth;
  const height = mapContainer.clientHeight;

  // Create canvas for Windy
  const canvas = document.createElement("canvas");
  canvas.width = width;
  canvas.height = height;
  canvas.style.position = "absolute";
  canvas.style.top = "0";
  canvas.style.left = "0";
  canvas.style.pointerEvents = "none";
  mapContainer.appendChild(canvas);

  // Initialize Windy with the canvas
  windyInstance = new W.maps.Windy({
    canvas: canvas,
    map: map,
    apiKey: options.key,
    overlays: {
      wind: true,
      temp: false,
      rain: false,
      clouds: false,
    },
    level: "surface",
    timestamp: Math.round(Date.now() / 1000),
  });

  // Update on map move
  map.on("moveend", updateWindy);
  updateWindy();
}

function updateWindy() {
  if (!windyInstance) return;

  const extent = map.getView().calculateExtent(map.getSize());
  const bounds = ol.proj.transformExtent(extent, "EPSG:3857", "EPSG:4326");

  windyInstance.update({
    bounds: {
      north: bounds[3],
      south: bounds[1],
      east: bounds[2],
      west: bounds[0],
    },
    zoom: map.getView().getZoom(),
  });
}

export function switchLayer(layer, mapForecastKey) {
  currentLayer = layer;
  const source = new ol.source.XYZ({
    url: `https://tiles.windy.com/tiles/v9.0/${layer}/{z}/{x}/{y}.png?key=${mapForecastKey}`,
    crossOrigin: "anonymous",
  });
  weatherLayer.setSource(source);
}

export function centerMap(lat, lon) {
  const coords = [lon, lat];
  map.getView().animate({
    center: ol.proj.fromLonLat(coords),
    zoom: 12,
    duration: 1000,
  });
  addMarker(coords);
}

export function getCurrentLocation() {
  if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(
      (position) => {
        const coords = [position.coords.longitude, position.coords.latitude];
        centerMap(position.coords.latitude, position.coords.longitude);
      },
      (error) => {
        console.error("Geolocation error:", error);
      }
    );
  }
}

// Load Windy API
function loadWindyAPI() {
  return new Promise((resolve, reject) => {
    if (window.W && window.W.maps && window.W.maps.Windy) {
      resolve();
      return;
    }

    const script = document.createElement("script");
    script.src = "https://api.windy.com/assets/map-forecast/libBoot.js";
    script.async = true;
    script.onload = () => {
      // Wait for Windy to be fully initialized
      const checkWindy = setInterval(() => {
        if (window.W && window.W.maps && window.W.maps.Windy) {
          clearInterval(checkWindy);
          resolve();
        }
      }, 100);

      // Timeout after 10 seconds
      setTimeout(() => {
        clearInterval(checkWindy);
        reject(new Error("Timeout waiting for Windy API"));
      }, 10000);
    };
    script.onerror = reject;
    document.head.appendChild(script);
  });
}

// Handle window resize
window.addEventListener("resize", () => {
  if (map) {
    map.updateSize();
    if (windyInstance) {
      const mapContainer = map.getTargetElement();
      const canvas = mapContainer.querySelector("canvas");
      if (canvas) {
        canvas.width = mapContainer.clientWidth;
        canvas.height = mapContainer.clientHeight;
        updateWindy();
      }
    }
  }
});

function addMarker(coords) {
  // Clear existing markers
  vectorLayer.getSource().clear();

  // Add new marker
  const feature = new ol.Feature({
    geometry: new ol.geom.Point(ol.proj.fromLonLat(coords)),
  });
  vectorLayer.getSource().addFeature(feature);
}
