# gateway-placetopay

Esta es una prueba para el Proceso de selección Analista de Implementación - Evertec Placetopay de ejemplo que muestra cómo utilizar la API de Placetopay para realizar pagos en línea utilizando Webcheckout.

## Configuración

Antes de ejecutar la aplicación, asegúrate de configurar los valores necesarios en el archivo `appsettings.json`. El archivo debe estar ubicado en la siguiente ruta:

`placetopay\WebCheckoutPlacetopay\bin\Debug\net6.0\appsettings.json`

El contenido del archivo `appsettings.json` debe tener la siguiente estructura:

```json
{
 "credential": {
 "Login": "IngreseSuLogin",
 "SecretKey": "IngreseSuSecretKey"
 },
 "request": {
 "description": "IngreseLaDescripcionDelPago",
 "currency": "IngreseLaMoneda",
 "total": 0.0,
 "returnUrl": "IngreseLaURLDeRetorno",
 "ipAddress": "IngreseLaDireccionIP",
 "userAgent": "IngreseElUserAgent"
 }
```

Asegúrate de reemplazar los valores `"IngreseSuLogin"`, `"IngreseSuSecretKey"`, `"IngreseLaDescripcionDelPago"`, `"IngreseLaMoneda"`, `0.0`, `"IngreseLaURLDeRetorno"`, `"IngreseLaDireccionIP"` y `"IngreseElUserAgent"` con la información correspondiente.

- `"Login"`: Ingresa tu login proporcionado por Placetopay.
- `"SecretKey"`: Ingresa tu Secret Key proporcionada por Placetopay.
- `"description"`: Ingresa una descripción para el pago.
- `"currency"`: Ingresa la moneda en la que se realizará el pago (por ejemplo, "USD", "COP", etc.).
- `"total"`: Ingresa el monto total del pago.
- `"returnUrl"`: Ingresa la URL a la que se redirigirá al usuario después de completar el pago.
- `"ipAddress"`: Ingresa la dirección IP del cliente que realiza el pago.
- `"userAgent"`: Ingresa el User-Agent del cliente que realiza el pago.

Asegúrate de completar todos los valores requeridos correctamente antes de ejecutar la aplicación.

## Cómo utilizar

1. Abre el archivo `appsettings.json` y configura los valores necesarios según se describe en la sección de configuración.

2. Ejecuta la aplicación `WebCheckoutPlacetopay`. Se realizará una solicitud de sesión de pago utilizando los parámetros proporcionados en el archivo de configuración.

3. La aplicación enviará la solicitud a la API de Placetopay y manejará la respuesta.

4. Si la respuesta indica un estado exitoso y contiene una URL de procesamiento, se abrirá esa URL en el navegador web para que el usuario pueda completar el pago.

5. La aplicación esperará periódicamente para verificar el estado de la sesión de pago. Una vez que el estado no sea "PENDING", se mostrará el estado final de la transacción.

## Dependencias

El proyecto utiliza las siguientes dependencias:

- Microsoft.Extensions.Configuration
- Newtonsoft.Json

## Requisitos

- .NET 6.0 SDK

---

Asegúrate de proporcionar los detalles necesarios en el archivo `appsettings.json` antes de utilizar la aplicación. Recuerda que este es solo un README de ejemplo, y puedes personalizarlo según tus necesidades.