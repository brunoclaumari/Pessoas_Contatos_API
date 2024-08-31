#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["Pessoas_API/Pessoas_API.csproj", "Pessoas_API/"]
#RUN dotnet restore "Pessoas_API/Pessoas_API.csproj"
#COPY . .
#WORKDIR "/src/Pessoas_API"
#RUN dotnet build "Pessoas_API.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Pessoas_API.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Pessoas_API.dll"]

#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
#WORKDIR /app
#
## Copia o arquivo "csproj" e restaura como layers distintos
#COPY *.csproj ./
#RUN dotnet restore
#
##Copia todas as coisas e então executa o build
#COPY . ./
#RUN dotnet publish -c Release -o out
#
##Faz o build da imagem do runtime
#FROM mcr.microsoft.com/dotnet/aspnet:6.0
#WORKDIR /app
#COPY --from=build-env /app/out .
#ENTRYPOINT ["dotnet", "Pessoas_API.dll"]
