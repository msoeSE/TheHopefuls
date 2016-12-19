# Requirements
* [Node version 6 or greater](https://nodejs.org/)
* npm (Installs with Node)
* [Bower](https://bower.io/)

# Setup

1. Download the repository
2. Install npm modules: `npm install`
3. Install bower dependencies `bower install`
4. Start up the server: `node server.js` or `npm start`
5. View in browser at http://localhost:8080

# Linting
1. `npm install -g gulp-cli`
2. `npm install`
3. `gulp lint`

# Production
1. `npm install`
2. `bower install`
3. Build with `gulp build` (This minifies all code and compresses image files)
4. Set environment variable (for respective OS) `NODE_ENV=production`
5. `npm start`
(Note: Don't develop in this unless you like having to do `gulp build` for every change)

# Testing
 1. To run the tests: `gulp test`
 2. To run tests on file change: `gulp tdd`
