[![Render Deploy](https://github.com/Malavos/conways-game-of-life-api/actions/workflows/render-deploy.yml/badge.svg?branch=master&event=check_run)](https://github.com/Malavos/conways-game-of-life-api/actions/workflows/render-deploy.yml)

# Conway's Game of Life API

This API provides endpoints to simulate Conway's Game of Life, implemented using ASP.NET Core and Entity Framework Core.

## Features

- Upload a new board state and get its ID.
- Get the next state, X states away, and final stable state of a board.

## Installation

1. Clone the repository:

    ```bash
    git clone <repository-url>
    cd conways-game-of-life-api
    ```

2. Restore packages:

    ```bash
    dotnet restore
    ```

3. Set up the database:

    ```bash
    dotnet ef database update
    ```

4. Run the application:

    ```bash
    dotnet run
    ```
  

## API Endpoints

### Upload a new board state

```json
POST /api/board
Content-Type: application/json

{
  "cells": [[true, false], [false, true]]
}
```

### Get the next state of a board

```json
GET /api/board/{boardId}/next
```

### Get X states away for a board

```json
GET /api/board/{boardId}/states/{x}
```

### Get the final state of a board

```json
GET /api/board/{boardId}/final
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.
