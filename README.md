# PrivateBlog

## Entity Framework Core (migraciones)

Ejecutar desde la carpeta del repositorio (donde está la solución).

**Instalar la herramienta global** (una vez):

```bash
dotnet tool install --global dotnet-ef
```

Para actualizarla:

```bash
dotnet tool update --global dotnet-ef
```

**Crear una migración:**

```bash
dotnet ef migrations add NombreDeLaMigracion --project PrivateBlog.Persistence --startup-project PrivateBlog.Web
```

**Aplicar migraciones a la base de datos:**

```bash
dotnet ef database update --project PrivateBlog.Persistence --startup-project PrivateBlog.Web
```

Los mismos `--project` y `--startup-project` aplican a otros subcomandos de `dotnet ef` (por ejemplo listar migraciones o generar scripts).
