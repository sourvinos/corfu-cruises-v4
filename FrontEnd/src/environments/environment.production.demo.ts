// ng build --output-path="release" --configuration=production-demo

export const environment = {
    apiUrl: 'https://appsourvinos.com/api',
    url: 'https://appsourvinos.com',
    appName: 'Corfu Cruises',
    clientUrl: 'https://appsourvinos.com',
    defaultLanguage: 'en-GB',
    featuresIconDirectory: 'assets/images/features/',
    nationalitiesIconDirectory: 'assets/images/nationalities/',
    portStopOrdersDirectory: 'assets/images/port-stop-orders/',
    cssUserSelect: 'auto',
    minWidth: 1280,
    login: {
        username: '',
        email: '',
        password: '',
        noRobot: false
    },
    isDevelopment: false,
    isProductionDemo: true,
    isProductionLive: false
}
