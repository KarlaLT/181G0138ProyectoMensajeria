//URL DE API
const urlAPI = "https://localhost:44362/api/";


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
            Password: usuario.value
        };

        let request = new Request(urlAPI + "usuarios/login", {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "*"
            },
            mode: 'no-cors',
            body: user
        });
        //var result = await fetch(request);

        //if (result.ok) { //solicitud exitosa
        //    let datos = await result.json();
        //}
        //else {
        //    console.log(result);
        //}
        localStorage.nameUsuario = "Héctor";
        window.location.href = "/Administrative";
    }
}

//ADMINISTRATIVE
let nameUsuario = document.querySelector("body header h1");
nameUsuario.innerText = "¡Bienvenido (a), " + localStorage.nameUsuario + "!";

