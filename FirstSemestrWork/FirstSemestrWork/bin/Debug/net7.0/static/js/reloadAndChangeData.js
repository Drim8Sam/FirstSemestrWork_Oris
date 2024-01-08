document.addEventListener('DOMContentLoaded', fetchData);

async function fetchData() {
    const response = await fetch('/getPersonages');
    const personageActive = await response.json();
    const personagesContainer = document.getElementById('personages-container');
    personagesContainer.innerHTML = '';

    // Список для хранения ссылок
    const links = [];

    for (let i = 0; i < personageActive.length; i++) {
        const personage = personageActive[i];
        const imagePathPersonage = personage.ImagePath;

        if (i % 4 === 0) {
            const rowContainer = document.createElement('div');
            rowContainer.classList.add('personages-row');
            personagesContainer.appendChild(rowContainer);
        }

        const personageContainer = document.createElement('div');
        personageContainer.classList.add('personage-item');

        personageContainer.innerHTML = `
            <tbody>
                <tr align="center">
                    <td>
                        <div style="position:relative; height: 152px; width: 152px;">
                            <div
                                style="position: relative; top: 140px; left: 0px; width: 152px; overflow: hidden; line-height: 12px; z-index: 4; text-align: center;">
                                <table cellspacing="0" cellpadding="1"
                                    style="background:#8B9286;border-radius: 4px 4px 4px 4px; -moz-border-radius: 4px 4px 4px 4px; -webkit-border-radius: 4px 4px 4px 4px;"
                                    align="center">
                                    <tbody>
                                        <tr>
                                        <td>&nbsp;<a href="#" id="characterLink${i + 1}" title="${personage.NamePersonage}">
                                        <font color="#FFFFFF" size="1">
                                            <b>${personage.NamePersonage}</b>
                                        </font>
                                    </a>&nbsp;</td>
                                    
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div
                                style="position: absolute; top: 5px; left: 0px; height: 152px; width: 152px; overflow: hidden;">
                                <div style="position: absolute; top: 7px; left: 7px; z-index: 2;">
                                    <a href="/personageInformation" title="${personage.NamePersonage}"><img
                                            src="${imagePathPersonage}"></a>
                                    <div id="border4"
                                        style="height: 152px;z-index: 5;position: absolute;width: 152px;top: -7px; left:-7px; font-size: 174px; overflow: hidden; line-height: 152px; z-index: 3">
                                        <a href="/personageInformation"
                                        title="${personage.NamePersonage}">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>`;

        const currentRow = personagesContainer.lastChild;
        currentRow.appendChild(personageContainer);


        // Добавление ссылки в список
        const link = document.getElementById(`characterLink${i + 1}`);
        links.push(link);
    }

    // Добавление обработчика событий для каждой ссылки
    links.forEach((link) => {
        link.addEventListener("click", function (event) {
            event.preventDefault();
            const headingText = event.currentTarget.innerText;
            changePersonage(headingText);
            console.log(headingText)
        });
    });
}

async function changePersonage(namePersonage) {
    try {
        let response = await fetch('/setCurrentPersonage', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            },
            body: JSON.stringify({
                NickName: namePersonage
            })

        });

        if (response.ok) {
            window.location.href = '/personageInformation';
        }

    } catch (error) {
        console.error('Ошибка:', error);
    }
}
