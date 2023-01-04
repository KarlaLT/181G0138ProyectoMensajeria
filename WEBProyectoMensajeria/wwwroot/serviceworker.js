const urlAPI = "https://mensajeriatecnm.itesrc.net/api/";

self.addEventListener("activate", function () {
    setInterval(function () {
        if (navigator.onLine) {
            reenviarRequestGuardadas();
        }
    }, 3600);
});


self.addEventListener("fetch", function (event) {
    event.respondWith(verificar(event)); //verificar si la solicitud se tiene ya guardada en caché o si es nueva para guardarla en cahcé
});


self.addEventListener("install", function () {
    var db = indexedDB.open("mensajesDB", 2);  //no utiliza promises ni async, utiliza callbacks

    db.onupgradeneeded = function () {
        //object store para guardar cambios en bd(post, put, delete)
        db.result.createObjectStore("peticiones", {
            autoIncrement: true, keyPath:"id"
        });
        //object store para requests que se hicieron cuando no se estaba online
        db.result.createObjectStore("cambios", {
            keyPath: "fecha"
        });
    };

});


async function verificar(event) { //CACHE FIRST

    if (event.request.url.includes("/api/")) { //es solicitud a api
        if (event.request.method == "GET") {
            let exist = await caches.match(event.request);

            if (exist) {
                //revalidar
                let clientid = event.clientId; //identificador de la pestaña del navegador (para saber qué pestaña se tiene que actualizar)
                revalidar(exist.clone(), clientid); //guardada está STALE

                return exist;
            }
            else {
                return cacheFirst(event.request);
            }
        }
        else {
            if (navigator.onLine) {
                return fetch(event.request);
            }
            else {
                guardarRequest(event.request); //guardar request en indexedDB mientras se logra enviar
                return new Response(null, { status: 200 });
            }
        }
    }
    else {
       // return cacheFirst(event.request);
        // return fetch(event.request);
        let exist = await caches.match(event.request);

        if (exist) {
            let clientid = event.clientId;
            revalidar(exist.clone(), clientid);
            return exist;
        }
        else {
            return cacheFirst(event.request);
        }
    }
}


async function revalidar(request, clientid) {
    let result = await fetch(request.url); //se hace de nuevo la petición a la dirección del request solicitdado
   // let clon = result.clone(); //clonarlo antes de leerlo, porque al leerlo se cierra el result

    //verificar si hay diferencias en el json que se tenía guardado antes y el nuevo json
    if (result.ok) {
        let clon = result.clone();
        let text = await result.text(); //viene de la api
        let staleText = await request.text(); //viene de la cache

        if (text != staleText) { //son diferentes
            let cache = await caches.open("cacheAPI"); //abrimos cache
            await cache.put(result.url, clon); //se guarda en cache la información que el request regresa

            //Avisar del cambio
            let ventana = await clients.get(clientid);
            ventana.postMessage({
                url: request.url,
                data: JSON.parse(text)
            });
        }
        //si son iguales no se hace nada, lo guardado en cache se queda igual a como estaba
    }
}


async function cacheFirst(request) {
    caches.match(request).then((cacheResponse) => {
        return cacheResponse || fetch(request).then((networkResponse) => {
            return caches.open("cacheAPI").then((cache) => {
                cache.put(request, networkResponse.clone());
                return networkResponse;
            })
        })
    })
};


async function guardarRequest(request) {
    let myRequest = {
        url: request.url,
        method: request.method,
        body: await request.json()
    };

    var response = indexedDB.open("mensajesDB");

    response.onsuccess = function (event) {
        var db = event.target.result;
        var transaction = db.transaction('peticiones', 'readwrite');
        var os = transaction.objectStore('peticiones');
        os.add(myRequest)
    }
}


async function reenviarRequestGuardadas() {
    var response = indexedDB.open("mensajesDB");

    response.onsuccess = function (event) {
        var db = event.target.result;
        var transaccion = db.transaction('peticiones', 'readwrite');
        var res = transaccion.objectStore('peticiones').getAll();

        res.onsuccess = async function () {
            let peticion = res.result;

            for (var i = 0; i < peticion.length; i++) {

                let response = await fetch(peticion[i].url, {
                    method: peticion[i].method,
                    body: JSON.stringify(peticion[i].body),
                    headers: {
                        "content-type": "application/json"
                    }
                });

                if (response.ok) {
                    var transaccion = db.transaction('peticiones', 'readwrite');
                    transaccion.objectStore('peticiones').delete(peticion[i].id);
                    i--;
                }
            }
        };
    };
}