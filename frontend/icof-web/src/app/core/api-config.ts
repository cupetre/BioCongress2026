// TODO: move to Angular environment files (or an injection token backed by build-time config)
// once there's a deployed backend URL to point at. For now this covers local dev only —
// whichever of the two backend run modes you're using (`dotnet run` on 5245, or the
// Dockerized API on 8080) as long as it matches what's actually running.
export const API_BASE_URL = 'http://localhost:5245';
