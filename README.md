# TestProject

A small playground with a C# minimal API (`api/`) and an Angular one-page UI (`web/`).

## Run

```sh
# terminal 1: API on http://localhost:5080
cd api && dotnet watch

# terminal 2: UI on http://localhost:5173 (proxies /api to the C# app)
cd web && npm run dev
```

Requires the .NET 10 SDK (`brew install --cask dotnet-sdk`) and Node.

`api/api.http` has sample requests you can send from VS Code or Rider.
