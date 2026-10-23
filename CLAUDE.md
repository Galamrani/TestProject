# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

A playground project: a C# ASP.NET Core minimal API (`api/`, .NET 10) and a one-page Angular UI (`web/`, Angular 22, standalone + signals). There are no tests or linters set up yet.

## Commands

```sh
# API: http://localhost:5080 (port set in api/Properties/launchSettings.json)
cd api && dotnet watch          # or: dotnet run / dotnet build

# UI: http://localhost:5173
cd web && npm run dev           # ng serve
cd web && npm run build         # type-check + production build to web/dist/web
```

The .NET SDK may not be installed on this machine (`brew install --cask dotnet-sdk`).

`api/api.http` contains sample requests for each endpoint.

## Architecture

- **How the UI reaches the API:** the browser calls relative `/api/...` URLs, and the Angular dev server forwards them to `http://localhost:5080` (`web/proxy.conf.json`, wired up in `angular.json` under `serve.options`). Because of that, the API has no CORS setup. If you change the API port, update both `launchSettings.json` and `web/proxy.conf.json`. Serving the built UI from somewhere other than the Angular dev server would need CORS or a reverse proxy.
- **API (`api/Program.cs`):** everything lives in this single file: endpoints mapped with `app.Map*`, plus the `Todo` / `NewTodo` records. Data is kept in a `ConcurrentDictionary` in memory and resets on restart. JSON uses ASP.NET's default camelCase, which the TypeScript `Todo` interface relies on.
- **UI:** `web/src/app/api.ts` is the typed client, an injectable `Api` service. Every call goes through `request<T>()` (plain `fetch`, not `HttpClient`), which throws using the server's `{ error }` message when one is present. `web/src/app/app.ts` + `app.html` is the single root component; state lives in signals and the todo list is reloaded after each change (`refresh()`). Errors from user actions are shown through the `run()` wrapper. Global styles are in `web/src/styles.css`.
- When adding an endpoint, update `Program.cs`, `web/src/app/api.ts` (with matching types) and `api/api.http` together.
