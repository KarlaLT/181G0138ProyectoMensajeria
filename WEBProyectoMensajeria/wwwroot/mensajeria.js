//URL DE API
const urlAPI = "https://mensajeriatecnm.itesrc.net/api/";
const plantillaMensaje = document.getElementById("plantillaMensaje");
const plantillaSent = document.getElementById("plantillaCRUD");
const sectionMsj = document.querySelector(".messages");
let datosUsuario;

//PÁGINA INDEX
var usuario = document.getElementById("correo");
var password = document.getElementById("password");
var btnLogin = document.querySelector(".login a");


if (btnLogin) {
    btnLogin.addEventListener("click", function () {
        Login();
    });

    async function Login() {
        let user = {
            Correo: usuario.value,
            Password: password.value
        };

        let request = new Request(urlAPI + "usuarios/login", {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "*"
            },
            mode: 'cors',
            body: JSON.stringify(user)
        });

        var result = await fetch(request);

        if (result.ok) { //solicitud exitosa, devuelve el token
            let token = await result.json();

            localStorage.token = token; //guardarlo en localstorage
            let array = token.split('.');
            let datos = array[1];

            // Decode the String
            var decodedStringAtoB = atob(datos);
            var json = JSON.parse(decodedStringAtoB)

            //guardar datos del usuario
            localStorage.nameUsuario = json.name;
            localStorage.idUsuario = json.nameid;
            localStorage.rol = json.role;

            if (localStorage.rol == "Estudiante") {
                window.location.href = "/Student";
            }
            else {
                window.location.href = "/Administrative";
            }
        }
        else {
            console.log(result);
        }
        
    }
}

//ADMINISTRATIVE/STUDENT
let nameUsuario = document.querySelector("body header .nameUser");
nameUsuario.innerText = "¡Bienvenido (a), " + localStorage.nameUsuario + "!";

if (sectionMsj) {

    if (plantillaSent) {
        async function getMensajesEnviados() {
            var result = await fetch(urlAPI + "mensajes/sent/" + localStorage.idUsuario, {
                method: "GET",
                mode: "cors",
                headers: new Headers({
                    "Authorization": "Bearer " + localStorage.token
                }),
            });

            //verificar resultado
            if (result.ok) {
                var mensajes = await result.json();
                mostrarDatos(mensajes, "enviados");
            }
            else {
                console.log(result);
            }
        }

        getMensajesEnviados();
    }
    else if (plantillaMensaje) {
        async function getMensajesRecibidos() {
            var result = await fetch(urlAPI + "mensajes/receive/" + localStorage.idUsuario, {
                method: "GET",
                mode: "cors",
                headers: new Headers({
                    "Authorization": "Bearer " + localStorage.token
                }),
            });

            //verificar resultado
            if (result.ok) {
                var mensajes = await result.json();
                mostrarDatos(mensajes, "recibidos");
            }
            else {
                console.log(result);
            }
        }

        getMensajesRecibidos();
    }

    async function mostrarDatos(mensajes, tipoMensajes) {
        //mostrarlos en vista con plantilla
        let cantidad = mensajes.length;

        if (cantidad > sectionMsj.children.length) {
            let n = cantidad - sectionMsj.children.length
            for (let x = 0; x < n; x++) { //si la cantidad  de datos es más grande de los ya existentes, sólo se agregan las plantillas que faltan
                if (tipoMensajes == "recibidos") {
                    let clon = plantillaMensaje.content.children[0].cloneNode(true);
                    sectionMsj.append(clon); //agregar a la tabla
                }
                else {
                    if (plantillaSent) {
                        let clon = plantillaSent.content.children[0].cloneNode(true);
                        sectionMsj.append(clon); //agregar a la tabla
                    }                 
                }              
            }

        }
        else if (cantidad < sectionMsj.children.length) { //si son menos, se eliminan las plantillas que sobran
            let n = sectionMsj.children.length - cantidad
            for (var x = 0; x < n; x++) {
                sec.lastChild.remove();
            }
        }

        //ya son iguales el num de plantillas y el num de datos, se llena con la info correcta actualizada
        mensajes.forEach((o, i) => { //i:indice, o:objeto 
            if (tipoMensajes == "recibidos") {
                getDatos(o.idEmisor, o, i, tipoMensajes);           
            }
            else {
                getDatos(o.idRemitente, o, i, tipoMensajes);           
            }
        });
    }

    async function getDatos(idUsuario, o, i, tipoMensaje) {
        var result = await fetch(urlAPI + "usuarios/" + idUsuario);

        if (result.ok) {
            datosUsuario = await result.json();
            //poner datos de usuario
            let div = sectionMsj.children[i];
            

            if (tipoMensaje == "recibidos") {
                if (datosUsuario) {
                    div.children[0].innerText = "De: " + datosUsuario.nombre;
                }
                else {
                    div.children[0].innerText = "No se localizaron datos del usuario.";
                }
                div.children[1].innerText = "Mensaje: " + o.mensaje1;
                div.children[2].innerText = o.fecha;
            }
            else {
                console.log(div);
                div.children[0].children[0].innerText = "Para: " + datosUsuario.nombre;
                div.children[1].innerText = "Mensaje: " + o.mensaje1;
                div.children[2].innerText = o.fecha;
            }
          
        }
        else {
            datosUsuario = null;
        }
    }
}

document.addEventListener("click", function (event){
    if (event.target.dataset.logout) {
        //cerrar sésión
        localStorage.removeItem("token");
        localStorage.removeItem("idUsuario");
        localStorage.removeItem("nameUsuario");
        window.location.href = event.target.dataset.logout;
    }

    //ver el modal que venga en el dataset del target
    if (event.target.dataset.modal) {
        document.getElementById(event.target.dataset.modal).style.top = 0;
    }
    //si el data-set es cancel
    if (event.target.dataset.cancel) {
        //Buscamos el elemento mas cercano que tenga el id .modal y lo escondemos.
        event.target.closest(".modal").style.top = '-100vh';
    }


});

