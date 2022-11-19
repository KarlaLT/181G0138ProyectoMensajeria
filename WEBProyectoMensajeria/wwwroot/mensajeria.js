//URL DE API
const urlAPI = "https://mensajeriatecnm.itesrc.net/api/";
const plantillaMensaje = document.getElementById("plantillaMensaje");
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

//ADMINISTRATIVE
let nameUsuario = document.querySelector("body header h1");
nameUsuario.innerText = "¡Bienvenido (a), " + localStorage.nameUsuario + "!";

if (plantillaMensaje) {

    async function getMensajesRecibidos() {
        var result = await fetch(urlAPI + "mensajes/receive/" + localStorage.idUsuario, {
            method:"GET",
            mode: "cors",
            headers: new Headers({
                'Authorization': 'Bearer ' + localStorage.token
            }),
        });

        //verificar resultado
        if (result.ok) {
            var mensajes = await result.json();
            console.log(mensajes);

            //mostrarlos en vista con plantilla
            let cantidad = mensajes.length;

            if (cantidad > sectionMsj.children.length) {
                let n = cantidad - sectionMsj.children.length
                for (let x = 0; x < n; x++) { //si la cantidad  de datos es más grande de los ya existentes, sólo se agregan las plantillas que faltan
                    let clon = plantillaMensaje.content.children[0].cloneNode(true);
                    sectionMsj.append(clon); //agregar a la tabla
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
                
                let div = sectionMsj.children[i];

                //localizar usuario emisor para poner su nombre en vista
                getUsuario(o.idEmisor);

                if (datosUsuario) {
                    div.children[0].innerText = "De: " + datosUsuario.nombre;
                }
                else {
                    div.children[0].innerText = "No se localizaron datos del usuario.";
                }
                div.children[1].innerText = "Mensaje: " + o.mensaje1;
                div.children[2].innerText = o.fecha;
                
            });

        }
        else {
            console.log(result);
        }
    }

    async function getUsuario(idUsuario) {
        var result = await fetch(urlAPI + "usuarios/" + idUsuario);

        if (result.ok) {
            datosUsuario = await result.json();
        }
        else {
            datosUsuario = null;
        }
    }

    getMensajesRecibidos();
}

