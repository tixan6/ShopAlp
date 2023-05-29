document.querySelector(".search-btn").addEventListener("click", function () {
    let inputValue = document.querySelector(".input-search").value.trim();
    let allItems = document.querySelectorAll('.forSearch');
    let allItemsForCheck = document.querySelector(".forSearch");
    var count = 0;
    var countItems = 0;
    allItems.forEach(function (CountItems) {
        if (CountItems) {
            countItems++;
        }
    })

    if (allItemsForCheck) {
        if (inputValue != "") {
            allItems.forEach(function (elem) {
                if (elem.innerText.search(inputValue) == -1) {
                    elem.classList.add('hide');

                    if (document.querySelector(".forSearch").classList.contains("hide")) {
                        count++;
                        if (count == countItems) {
                            Swal.fire('Такого товара нет')
                        }
                    }
                }
                else {
                    elem.classList.remove('hide');
                }
            });
        }
        else {
            allItems.forEach(function (elem) {
                elem.classList.remove('hide');
            });
        }
    }
    else {
        Swal.fire('Перейдите в каталог для поиска товара')
    }

})
