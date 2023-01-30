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