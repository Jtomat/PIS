$('.sortField').on('click', function (e) {
    e.stopPropagation();
    let cardsId = document.querySelectorAll("[class='idField']");
    resultArray = [];
    for (var i = 0; i < cardsId.length; i++) {
        resultArray.push(Number(cardsId[i].innerText))
    }
    let formData = new FormData()
    formData.append("filterCards", resultArray)

    $.ajax({
        url: $(this).attr('href'),
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            let parser = new DOMParser();
            let htmlDoc = parser.parseFromString(response, 'text/html');
            let newTable = htmlDoc.getElementById("tableID");
            let oldTable = document.getElementById("tableID");
            oldTable.parentNode.removeChild(oldTable);
            document.getElementById('tableContainer').appendChild(newTable)
        }
    });
}); 