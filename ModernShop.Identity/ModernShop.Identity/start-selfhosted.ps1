dapr run `
    --app-id identityservice `
    --app-port 6005 `
    --dapr-http-port 3605 `
    --dapr-grpc-port 60005 `
    --config ../../dapr/config/config.yaml `
    --config ../../dapr/config/kibana.yaml `
    --components-path ../../dapr/components `
    dotnet run