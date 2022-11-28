self.addEventListener("fetch", function (event) {
    event.respondWith(verificar(event)); //verificar si la solicitud se tiene ya guardada en caché o si es nueva para guardarla en cahcé
});

async function verificar(event) { //CACHE FIRST

    if (event.request.url.includes("/api/") && event.request.method == "GET") { //es solicitud a api

        //abrir cache y guardar
        let cache = await caches.open("cacheAPI");
        let exist = await cache.match(event.request);

        if (exist) {
            //revalidar
            let clientid = event.clientid; //identificador de la pestaña del navegador (para saber qué pestaña se tiene que actualizar)

            revalidar(exist.clone(), clientid); //guardada está STALE
            exist = await cache.match(event.request);
            return exist;
        }
        else {
            //guardarla y regresar lo que guardaste
            let result = await fetch(event.request);
            await cache.put(event.request, result.clone()); //el resultado se clona para que no se cierre y poder regresarlo enla sig instrucción
            return result;
        }
    }
    else {
        return await fetch(event.request);
    }
}



async function revalidar(request, clientid) {
    let result = await fetch(request.url); //se hace de nuevo la petición a la dirección del request solicitdado
    let clon = result.clone(); //clonarlo antes de leerlo, porque al leerlo se cierra el result

    //verificar si hay diferencias en el json que se tenía guardado antes y el nuevo json
    if (result.ok) {
        let text = await result.text(); //viene de la api
        let staleText = await request.text(); //viene de la cache

        if (text != staleText) { //son diferentes
            let cache = await caches.open("cacheAPI"); //abrimos cache
            await cache.put(result.url, clon); //se guarda en cache la información que el request regresa

            
            ////AVISAR DEL CAMBIO AL CLIENTE POR MEDIO DE SERVICE WORKER
            //let ventana = await clients.get(clientid);

            //ventana.postMessage({
            //    url: request.url,
            //    data: JSON.parse(text)
            //});
        }
        //si son iguales no se hace nada, lo guardado en cache se queda igual a como estaba
    }
}