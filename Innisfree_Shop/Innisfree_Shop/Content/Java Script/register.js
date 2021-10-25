var register= document.getElementById("myregister");
var btn = document.getElementById("signupbtn");
var span = document.getElementById("close-register");
btn.onclick = function () {
    register.style.display = "block";
}
span.onclick=function(){
    register.style.display = "none";
}
window.onclick=function(event){
    if (event.target == register)
        register.style.display = "none";
}