dapr run `
    --app-id catalogservice `
    --app-port 6001 `
    --dapr-http-port 3601 `
    --dapr-grpc-port 60001 `
    --config ../../dapr/config/config.yaml `
    --components-path ../../dapr/components `
    dotnet run