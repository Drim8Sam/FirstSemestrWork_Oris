document.getElementById("send-form").addEventListener("submit", async (event) => {
    event.preventDefault();
    var myFormData = new FormData(event.target);

    var user = {};
    myFormData.forEach((value, key) => (user[key] = value));
    console.log(user);

    let response = await fetch('/signin', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(user)
    });
    console.log(response);

    if (response.ok) {
        window.resultMessage.innerText = "Вы успешно вошли в систему. " 
            ;
        setTimeout(function () {
            window.location.href = "/admin";
        }, 2000);
    }
    else {
        window.resultMessage.innerText = "Неверно введен логин или пароль";
    }
});