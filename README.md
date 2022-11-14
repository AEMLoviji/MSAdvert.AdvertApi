# WebAdvert.AdvertApi

## AdvertApi.Models

Project stores models which are shared between AdvertApi and AdvertWeb.

### Build and push AdvertApi.Models package

Project settings configured to generate Package file (*.nupkg) on each build. Execute below command in order to push the generated package to `Nuget.org`.

> @nuget-api-key used below should be obtained from nuget.org portal.

```
dotnet nuget push AdvertApi.Models.EA.1.0.0.nupkg -k <nuget-api-key> -s https://api.nuget.org/v3/index.json
```