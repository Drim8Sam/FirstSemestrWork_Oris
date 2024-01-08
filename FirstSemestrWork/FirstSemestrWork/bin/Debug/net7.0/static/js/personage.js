document.addEventListener('DOMContentLoaded', fetchData);

async function fetchData() {
    try {
        const namePersonage = document.getElementById('name_personage');
        const kanji = document.getElementById('kanji');
        const romadzi = document.getElementById('romadzi');
        const nicknames = document.getElementById('nicknames');
        const race = document.getElementById('race');
        const gender = document.getElementById('gender');
        const birthday = document.getElementById('birthday');
        const age = document.getElementById('age');
        const height = document.getElementById('height');
        const hairColor = document.getElementById('hair_color');
        const eyeColor = document.getElementById('eye_color');
        const appearancePage = document.getElementById('appearance_page');
        const personalityPage = document.getElementById('personality_page');
        const capabilitiesPage = document.getElementById('capabilities_page');
        const weight = document.getElementById('weight');
        const mainPage = document.getElementById('main_page');

        const response = await fetch('/getCurrentPersonage');
        console.log(response)
        if (response.ok) {
            const currentPersonage = await response.json();
            console.log(currentPersonage)

            if (currentPersonage) {
                
                namePersonage.innerText = `${currentPersonage.NamePersonage}`;
                kanji.innerText = `${currentPersonage.Kanji}`;
                romadzi.innerText = `${currentPersonage.Romadzi}`;
                nicknames.innerText = `${currentPersonage.Nicknames}`;
                race.innerText = `${currentPersonage.Race}`;
                gender.innerText = `${currentPersonage.Gender}`;
                birthday.innerText = `${currentPersonage.Birthday}`;
                age.innerText = `${currentPersonage.Age}`;
                height.innerText = `${currentPersonage.Height}`;
                hairColor.innerText = `${currentPersonage.HairColor}`;
                eyeColor.innerText = `${currentPersonage.EyeColor}`;
                appearancePage.innerText = `${currentPersonage.AppearancePage}`;
                personalityPage.innerText = `${currentPersonage.PersonalityPage}`;
                capabilitiesPage.innerText = `${currentPersonage.CapabilitiesPage}`;
                
                mainPage.innerText = `${currentPersonage.MainPage}`;

                
                const imageContainer = document.getElementById('Image_Path');
                imageContainer.innerHTML = `
                    <figure class="pi-item pi-image">
                        <a href="" class="image image-thumbnail" title="Аниме" style="display: inline-grid;">
                            <img src="${currentPersonage.ImagePath}" width="270" height="401">
                            <figcaption class="pi-item-spacing pi-caption">
                                <a class="view-image-link">
                                    <span style="display: inline-block;">
                                        <span class="gallery-icon-container view-image">
                                            <span class="icon-container view-image" aria-label="просмотр изображения">
                                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"
                                                    viewBox="0 0 18 18" width="1em" height="1em">
                                                    <defs>
                                                        <path id="IconImagesSmall__a"
                                                            d="M11 15h-.586L3 7.586V7h8v8zm-8 0v-4.586L7.586 15H3zm9-10a1 1 0 011 1v10a1 1 0 01-1 1H2a1 1 0 01-1-1V6a1 1 0 011-1h10zm4-4a1 1 0 011 1v10a1 1 0 11-2 0V3H6a1 1 0 110-2h10z">
                                                        </path>
                                                    </defs>
                                                    <use xlink:href="#IconImagesSmall__a" fill-rule="evenodd"></use>
                                                </svg>
                                            </span>
                                            просмотр изображения
                                        </span>
                                    </span>
                                </a>
                            </figcaption>
                        </a>
                    </figure>
                `;
            } else {
                console.error('Server response did not contain valid data for the current personage.');
            }
        } else {
            console.error('Error fetching current personage data from the server.');
        }
    } catch (error) {
        console.error('An error occurred while fetching or processing data:', error);
    }
}
