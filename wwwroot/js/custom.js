let index = 0;

function AddTag() {
    var tagEntry = document.getElementById("TagEntry");

    //create new Select option
    let newOption = new Option(tagEntry.value, tagEntry.value);
    document.getElementById("TagList").options[index++] = newOption;

    //clear the entry box
    tagEntry.value = "";
    return true;


}
function DeleteTag() {
    let tagList = document.getElementById("TagList");
    let tagCount = 1;
    while (tagCount > 0) {
        let selectedIndex = tagList.selectedIndex;
        if (selectedIndex >= 0) {
            tagList.options[selectedIndex] = null;
            --tagCount;
        }
        else
            tagCount = 0;
        index--;
    }
}
//jQuery:
$("form").on("submit", function () {
    $("#TagList option").prop("selected", "selected");
})

if (tagValues != "") {
    let tagArray = tagValues.split(",");
    for (let loop = 0; loop < tag.length; loop++) {
        ReplaceTag(tagArray[loop], loop);
        index++;
    }
}

function ReplaceTag(tag, index) {
    let newOption = new Option(tag, tag);
    document.getElementById("TagList").options[index] = newOption;
}