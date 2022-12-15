//URL DE API
const urlAPI = "https://mensajeriatecnm.itesrc.net/api/";
const plantillaMensaje = document.getElementById("plantillaMensaje");
const plantillaSent = document.getElementById("plantillaCRUD");
const sectionMsj = document.querySelector(".messages");
let datosUsuario;
let usuarios;
let grupos;
let clases;
let carreras; 

//PÁGINA INDEX
var usuario = document.getElementById("correo");
var password = document.getElementById("password");
var btnLogin = document.querySelector(".login a");

navigator.serviceWorker.addEventListener("message", function (event) {
    if (event.data.includes(urlAPI + "mensajes/")) {
        mostrarDatos(event.data.data);
    }
});

if (btnLogin) {
    btnLogin.addEventListener("click",  function () {
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
    if (!usuarios) {
         getUsuarios();
    }
    if (!grupos) {
        getGrupos();
    }
    if (!clases) {
        getClases();
    }
    if (!carreras) {
        getCarreras();
    }
    if (plantillaSent) {       
        getMensajesEnviados();
    }
    else if (plantillaMensaje) {  
        getMensajesRecibidos();
    }
}

document.addEventListener("click", async function (event){
    if (event.target.dataset.logout) {
        //cerrar sésión
        localStorage.removeItem("token");
        localStorage.removeItem("idUsuario");
        localStorage.removeItem("nameUsuario");
        window.location.href = event.target.dataset.logout;
    }

    //ver el modal que venga en el dataset del target
    if (event.target.dataset.modal) {
        let modal = document.getElementById(event.target.dataset.modal);
        if (event.target.dataset.modal.includes("updateMessage")) {
            //get al elemento que se quiere editar de la base de datos
            let id = event.target.parentNode.parentNode.dataset.idvermensaje;
            console.log(id);
            var result = await fetch(urlAPI + "mensajes/" + id, {
                method: "GET",
                headers: new Headers({
                    "Authorization": "Bearer " + localStorage.token
                })
            });

            if (result.ok) {
                let json = await result.json();

                let form = modal.querySelector("form");
                if (plantillaMensaje) {
                    form.children[1].children[1].innerText = usuarios.find(x => x.id == json.idEmisor).nombre;
                }
                else if (plantillaSent) {
                    form.elements["IdRemitente"].value = usuarios.find(x => x.id == json.idRemitente).nombre;
                    form.elements["IdRemitenteHidden"].value = json.idRemitente;
                    form.elements["IdMensaje"].value = json.id;
                }
                form.elements["Mensaje"].innerText = json.mensaje1;

            }
            else {
                console.log(json);
            }
        }
        if (event.target.dataset.modal.includes("deleteMessage")) {
            let id = event.target.parentNode.parentNode.dataset.idvermensaje;
            console.log(id);

            let form = modal.querySelector("form");
            form.elements["IdMensaje"].value = id;
        }

        document.getElementById(event.target.dataset.modal).style.top = 0;
    }
    //si el data-set es cancel
    if (event.target.dataset.cancel) {
        //Buscamos el elemento mas cercano que tenga el id .modal y lo escondemos.
        event.target.closest(".modal").style.top = '-100vh';
    }
    //si el data es para ver un mensaje
    if (event.target.dataset.idvermensaje) {
        verMensaje(event.target.dataset.idvermensaje);
    }
    //si eestá mandando llamar a un filtro
    if (event.target.dataset.filtro) {
        filtrar(event.target.dataset.filtro);
    }

});


async function getMensajesEnviados() {
    var result = await fetch(urlAPI + "mensajes/sent/" + localStorage.idUsuario, {
        method: "GET",
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

async function getMensajesRecibidos() {
    var result = await fetch(urlAPI + "mensajes/receive/" + localStorage.idUsuario, {
        method: "GET",
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

async function verMensaje(idMensaje) {
    var result = await fetch(urlAPI + "mensajes/"+idMensaje, {
        method: "GET",
        headers: new Headers({
            "Authorization": "Bearer " + localStorage.token
        })
    });

    if (result.ok) {
        let json = await result.json();
        let modal = document.getElementById("seeMessage");
        let form = modal.children[0];
        if (plantillaMensaje) {
            form.children[1].children[1].innerText =usuarios.find(x => x.id == json.idEmisor).nombre; 
        }
        else if (plantillaSent) {
            form.children[1].children[1].innerText = usuarios.find(x => x.id == json.idRemitente).nombre;
        }
        form.children[3].innerText = json.mensaje1;

        modal.style.top = 0;
    }
    else {
        console.log(result);
    }
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
        div.style.display = "block";

        if (tipoMensaje == "recibidos") {
            if (datosUsuario) {
                div.children[0].innerText = "De: " + datosUsuario.nombre;
            }
            else {
                div.children[0].innerText = "No se localizaron datos del usuario.";
            }
            div.children[1].innerText = "Mensaje: " + o.mensaje1;
            const fecha = new Date(o.fecha);
            div.children[2].innerText = fecha.toLocaleString();
        }
        else {
            div.children[0].children[0].innerText = "Para: " + datosUsuario.nombre;
            div.children[1].innerText = "Mensaje: " + o.mensaje1;
            const fecha = new Date(o.fecha);
            div.children[2].innerText = fecha.toLocaleString();
        }
        div.setAttribute("data-idVerMensaje", o.id);
        var children = Array.from(div.children);
        children.forEach(x => x.setAttribute("data-idVerMensaje", o.id));
    }
    else {
        datosUsuario = null;
    }
}


document.addEventListener("submit", async function (event) {
    //cancelar el evento submit
    event.preventDefault();

    let form = event.target;
    let json;
    let request;

    //FormData estan todos los names que tiene el formulario
    if (event.target.dataset.modal == "updateMessage") {
        json = {
            Id: form.elements["IdMensaje"].value,
            IdEmisor: localStorage.idUsuario,
            IdRemitente: form.elements["IdRemitenteHidden"].value,
            Mensaje1: form.children[5].value
        };

        //Hacemos un fetch a la API y para hacer por POST debemos pasar el RequestInfo y eso va en las llavesitas
        request = new Request(urlAPI + form.dataset.action, {
            method: "PUT",
            //El cuerpo siempre debe ser string no permite enviar un json.
            body: JSON.stringify(json),
            //SI la api te obliga a que le pongas JSON
            headers: {
                'Content-type': 'application/json',
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': '*'
            },
            mode: "cors"
        });
    }
    else if (event.target.dataset.modal == "deleteMessage") {
        request = new Request(urlAPI + form.dataset.action + "/" + form.elements["IdMensaje"].value, {
            method: "DELETE",
            mode: "cors"
        });
    }
    else {
        json = {
            idEmisor: localStorage.idUsuario,
            idRemitente: form.children[1].children[1].value,
            mensaje1: form.children[3].value
        };

        //Hacemos un fetch a la API y para hacer por POST debemos pasar el RequestInfo y eso va en las llavesitas
        request = new Request(urlAPI + form.dataset.action, {
            method: form.method,
            //El cuerpo siempre debe ser string no permite enviar un json.
            body: JSON.stringify(json),
            //SI la api te obliga a que le pongas JSON
            headers: {
                'Content-type': 'application/json',
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': '*'
            },
            mode: "cors"
        });
    }
   
    

    var result = await fetch(request);
    if (result.ok) {
        window.location.reload();
    }
    else {
        let error = await result.json();
        console.log(error);
    }
});

//llenar select de usuarios para seleccionar remitente
async function getUsuarios() {
    var result = await fetch(urlAPI + "usuarios");

    if (result.ok) {
        usuarios = await result.json();

        var selects = document.querySelectorAll("[name=IdRemitente]");
        if (selects) {
            selects.forEach(select => {
                if (select.nodeName == "SELECT") {
                    usuarios.forEach(x => {
                        let option = document.createElement("OPTION");
                        option.innerText = x.nombre;
                        option.value = x.id;
                        select.options.add(option);
                    })
                }                
            });
        }        
    }
}

async function getGrupos() {
    var result = await fetch(urlAPI + "usuarios/grupos");

    if (result.ok) {
        grupos = await result.json();
        console.log(grupos);
        //var selects = document.querySelectorAll("[name=IdRemitente]");
        //if (selects) {
        //    selects.forEach(select => {
        //        usuarios.forEach(x => {
        //            let option = document.createElement("OPTION");
        //            option.innerText = x.nombre;
        //            option.value = x.id;
        //            select.options.add(option);
        //        })
        //    });
        //}
    }
}

async function getCarreras() {
    var result = await fetch(urlAPI + "usuarios/carreras");

    if (result.ok) {
        carreras = await result.json();
        console.log(carreras);
        //var selects = document.querySelectorAll("[name=IdRemitente]");
        //if (selects) {
        //    selects.forEach(select => {
        //        usuarios.forEach(x => {
        //            let option = document.createElement("OPTION");
        //            option.innerText = x.nombre;
        //            option.value = x.id;
        //            select.options.add(option);
        //        })
        //    });
        //}
    }
}

async function getClases() {
    var result = await fetch(urlAPI + "usuarios/clases");

    if (result.ok) {
        clases = await result.json();
        console.log(clases);
        //var selects = document.querySelectorAll("[name=IdRemitente]");
        //if (selects) {
        //    selects.forEach(select => {
        //        usuarios.forEach(x => {
        //            let option = document.createElement("OPTION");
        //            option.innerText = x.nombre;
        //            option.value = x.id;
        //            select.options.add(option);
        //        })
        //    });
        //}
    }
}

function filtrar(parametro) {
    let hoy = new Date();

    if (parametro == "hoy") {
        let array = new Array();

        for (var i = 0; i < sectionMsj.children.length; i++) {
            let child = sectionMsj.children[i];
            //children 2 en recibidos y enviados  
            if (child.children[2].innerText.includes(hoy.toLocaleDateString())) {
                array.push(child);
            }
        }
        for (var i = 0; i < sectionMsj.children.length; i++) {
            let child = sectionMsj.children[i];
            if (i < array.length) {
                //reemplazar renglones de la tabla por los filtrados
                child == array[i];
            }
            else {
                //ocultar renglones que no se necesitan
                child.style.display = "none";
            }
        }
    }
    else if (parametro == "ayer") {
        let yesterday = new Date(hoy);
        yesterday.setDate(yesterday.getDate() - 1);
        console.log(yesterday.toLocaleDateString());

        let array = new Array();

        for (var i = 0; i < sectionMsj.children.length; i++) {
            let child = sectionMsj.children[i];
            //children 2 en recibidos y enviados  
            if (child.children[2].innerText.includes(yesterday.toLocaleDateString())) {
                array.push(child);
            }
        }
        for (var i = 0; i < sectionMsj.children.length; i++) {
            let child = sectionMsj.children[i];
            if (i < array.length) {
                //reemplazar renglones de la tabla por los filtrados
                child == array[i];
            }
            else {
                //ocultar renglones que no se necesitan
                child.style.display = "none";
            }
        }
          
    }
    else if (parametro == "todos") {
        getMensajesRecibidos();
    }
} //funcional