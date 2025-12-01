ZSport UI
=========

Interfaz web de ZSports, una plataforma para la gestión de canchas deportivas, superficies y perfiles de usuarios/propietarios. Este proyecto proporciona el frontend en Angular, centrado en una experiencia moderna, responsiva y fácil de usar.

**🎯 Objetivo del proyecto**

- Ofrecer una UI clara para la administración de:
	- Canchas (`Canchas`)
	- Superficies (`Superficies`)
	- Perfiles de usuarios y propietarios (`profile`)
- Integrarse con los servicios de backend de ZSports para:
	- Autenticación de usuarios
	- Navegación y autorización basada en roles
	- Gestión de entidades deportivas
- Servir como base escalable para seguir incorporando nuevas funcionalidades de ZSports.

**🧱 Arquitectura y estructura**

El proyecto sigue una arquitectura modular basada en características:

- `src/app/app.config.ts`: configuración principal de la aplicación (providers, routing, etc.).
- `src/app/app.routes.ts`: definición de rutas principales (páginas como `Canchas`, `Superficies`, `profile`, etc.).
- `src/app/auth`: módulo de autenticación:
	- `auth.service.ts`: lógica de autenticación y comunicación con backend.
	- `auth.guard.ts`: protección de rutas (AuthGuard).
	- `auth.store.ts`: manejo de estado de autenticación.
	- `auth.type.ts`: tipos/interfaces relacionados.
- `src/app/pages`:
	- `Canchas/`: página para gestionar canchas.
	- `Superficies/`: página para gestionar superficies, con componentes hijos para crear/editar.
	- `profile/`: página de perfil con subcomponentes para:
		- `owner-info`
		- `profile-info`
- `src/app/shared`:
	- `buttons/`: componentes de botón reutilizables (botón estándar, dropdown, icon-button, etc.).
	- `loading/`: componente de loading/spinner.
	- `sidebar/`: componente de menú lateral.
	- `snackbar/`: sistema de notificaciones.
	- `toolbar/`: barra superior de la aplicación.
- `src/environments/`:
	- Distintos archivos de entorno (`development`, `staging`, `production`, `docker-dev`) para configurar URLs de APIs y otras variables.

**🛠️ Tecnologías utilizadas**

**Frontend**

- **Angular** (versión según `package.json`):
	- Standalone components (según estructura `app.config.ts` y `app.routes.ts`).
	- Angular Router para manejo de rutas y guards.
- **TypeScript**:
	- Tipado estático para servicios, stores y componentes.
- **SCSS**:
	- Estilos modulares por componente (`.scss` junto a cada `.ts`).
	- Estilos globales en `src/styles.scss`.

**Gestión de estado y servicios**

- **Servicios Angular (`@Injectable`)**:
	- `auth.service.ts`, `navigation.service.ts`, servicios de dominios (`superficies.service.ts`, etc.).
- **Stores propios**:
	- `auth.store.ts`, `superficies.store.ts` para encapsular el estado de cada feature.
- **Tipos/interfaces**:
	- Definidos en `types/` de cada feature (por ejemplo, `superficies.types.ts`, `auth.type.ts`).

**Entornos y configuración**

- **`environment.*.ts`**:
	- Configuración de endpoints y flags por entorno (`development`, `staging`, `production`, `docker-dev`).
- **`tsconfig.*.json`**:
	- Configuración de compilación para app, tests y entorno Docker (`tsconfig.app.json`, `tsconfig.spec.json`, `tsconfig.docker-dev.json`).

**🚀 Puesta en marcha**

**Requisitos previos**

- Node.js (versión recomendada acorde al `package.json` del proyecto).
- npm (o pnpm/yarn, según se utilice en el equipo).

**Instalación de dependencias**

Desde la raíz del proyecto (`zsport-ui`):

```bash
npm install
```

**Ejecutar en desarrollo**

```bash
npm start
# o
npm run start
```

Por defecto, Angular se levantará en un puerto como `http://localhost:4200` (según configuración de `angular.json`).

**Ejecutar tests**

```bash
npm test
# o
npm run test
```

**🌐 Entornos**

El proyecto soporta múltiples entornos definidos en `src/environments/`:

- `environment.development.ts`: entorno de desarrollo local.
- `environment.staging.ts`: entorno de pruebas / staging.
- `environment.production.ts`: entorno de producción.
- `environment.docker-dev.ts`: entorno pensado para ejecución en contenedor.
- `environment.ts`: archivo de referencia para el build por defecto.

La selección del entorno se realiza mediante configuración de `angular.json` (build configurations).

**📦 Scripts principales (npm)**

Revisa `package.json` para la lista completa; típicamente incluye:

- `start`: inicia la aplicación en modo desarrollo.
- `build`: genera el build de producción.
- `test`: ejecuta la suite de tests.

Ejemplo:

```bash
npm run build
```

**📁 Componentes compartidos destacados**

- `shared/buttons`: sistema de botones reutilizables para mantener consistencia visual.
- `shared/snackbar`: notificaciones contextuales (éxito, error, información).
- `shared/sidebar` y `shared/toolbar`: layout principal de navegación.
- `shared/loading`: indicador de carga reutilizable.

**🔐 Autenticación y seguridad**

- Rutas protegidas mediante `AuthGuard` (`auth/guard/auth.guard.ts`).
- Estado de autenticación centralizado en `auth.store.ts`.
- `auth.service.ts` coordinando llamadas al backend (login, logout, refresh, etc., según implementación).

**🧩 Navegación**

- `navigation.service.ts` centraliza lógica de navegación y manejo de rutas.
- Rutas principales definidas en `app.routes.ts`, separadas por páginas (`Canchas`, `Superficies`, `profile`, `login`, etc.).

**🤝 Contribución**

1. Crea una rama desde `dev` para tu feature o fix.
2. Añade o actualiza tests si corresponde.
3. Asegúrate de que `npm test` pasa correctamente.
4. Envía un Pull Request contra `dev` con una descripción clara de los cambios.
