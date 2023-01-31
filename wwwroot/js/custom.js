// Constants
//the selected property
const SELECTED = "selected";

// Custom alert
const swalWithDarkButton = Swal.mixin({
    customClass: {
        confirmButton: `btn btn-danger btn-sm btn-block btn-outline-dark`
    },
    imageUrl: '/img/doh.jpg',
    imageHeight: 200,
    imageAlt: 'Homer Simpson: saying, "Doh!"',
    timer: 3000,
    buttonsStyling: false
});

// Variables
let index = 0;

// Functions

// Add tag function
function AddTag() {
    // Use search to detect duplicate tags
    let tagEntry = document.getElementById("TagEntry");
    if (!tagEntry.value) {
        showError("EMPTY tags are not allowed.");
        return false;
    }
    let searchResult = Search(tagEntry.value);

    if (searchResult !== null) {
        showError(`#${searchResult} was found to be a DUPLICATE tag for this post. Please enter a new tag.`);
        return false;
    } else {
        // Create a new Select option
        let newOption = new Option(tagEntry.value, tagEntry.value);
        document.getElementById("TagList").options[index++] = newOption;
    }

    // Clear the entry box
    tagEntry.value = "";
    return true;
}

// Delete tag function
function DeleteTag() {
    let tagList = document.getElementById("TagList");
    if (!tagList) return false;

    if (tagList.selectedIndex === -1) {
        showError("No tags selected to delete");
        return true;
    }

    while (tagList.selectedIndex >= 0) {
        tagList.options[tagList.selectedIndex] = null;
        index--;
    }
}

//replace tag function
function ReplaceTag(tag, position) {
    let newOption = new Option(tag, tag);
    document.getElementById("TagList").options[position] = newOption;
}

//search function
function Search(str) {
    let tagsEl = document.getElementById("TagList");
    if (tagsEl) {
        let tags = tagsEl.options;
        for (let i = 0; i < tags.length; i++) {
            if (tags[i].value === str) {
                return str;
            }
        }
    }
    return null;
}

//show alert
function showError(message) {
    //Swal.fire({
    swalWithDarkButton.fire({
        title: "Error",
        text: message,
        icon: "error",
        timer: 3000,
        customClass: {
            confirmButton: "btn btn-danger btn-sm btn-block btn-outline-dark"
        },
        buttonsStyling: false
    });
}

// Event Listeners
$("form").on("submit", function () {
    $("#TagList option").prop(SELECTED, SELECTED);
});

if (typeof tagValues !== "undefined" && tagValues !== "") {
    let tagArray = tagValues.split(",");
    for (let i = 0; i < tagArray.length; i++) {
        ReplaceTag(tagArray[i], i);
        index++;
    }
}
