# Requirements
* [Node version 6 or greater](https://nodejs.org/)
* npm (Installs with Node)

# Setup

1. Download the repository
2. Install npm modules: `npm install`
3. Start up the server: `node server.js` or `npm start`
4. View in browser at http://localhost:3000

# Linting
1. `npm install -g gulp-cli`
2. `npm install`
3. `gulp lint`

# Production
1. `npm install`
2. Build with `gulp build` (This minifies all code and compresses image files)
3. Set environment variable (for respective OS) `NODE_ENV=production`
4. Start up the server: `node server.js` or `npm start`
(Note: Don't develop in this unless you like having to do `gulp build` for every change)
