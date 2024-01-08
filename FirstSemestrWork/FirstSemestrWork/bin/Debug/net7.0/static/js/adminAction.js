document.addEventListener('DOMContentLoaded', fetchData);

async function fetchData() {
    const response = await fetch('/getPersonages');
    const personages = await response.json();
    const personagesContainer = document.getElementById('Active_personage');

    personagesContainer.innerHTML = '';

    personages.forEach(personage => {
        personagesContainer.innerHTML += `<li>${personage.NamePersonage}</li>`;
    });
}

function goToHomePage() {
    window.location.href = "/home";
}

async function addCharacter() {
    var form = document.getElementById('addCharacterForm');
    var name = form.elements.characterName.value;
    var imagePath = form.elements.characterImagePath.value;

    let response = await fetch('/addPersonage', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ NickName: name, ImagePath: imagePath})
    });

    if (response.ok) {
        form.reset();
        fetchData();
    }
}


async function deleteCharacter() {
    var nameToDelete = document.getElementById('deleteCharacterName').value;

    let response = await fetch('/deletePersonage', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ namePersonage: nameToDelete })
    });

    if (response.ok) {
        document.getElementById('deleteCharacterName').value = '';
        fetchData();
    }
}


