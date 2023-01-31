//let index = 0;

//function AddTag() {
//    //use search to detact duplicate tags
//    var tagEntry = document.getElementById("TagEntry");
//    let searchResult = null;
//    searchResult = Search(tagEntry.value);

//    if (searchResult != null) {
//        swalWithDarkButton.fire({
//            html: `<span class='font-weight-bolder'>${searchResult.toUpperCase()}</span>`
//        });
//    }
//    else {
//        //create new Select option
//        let newOption = new Option(tagEntry.value, tagEntry.value);
//        document.getElementById("TagList").options[index++] = newOption;
//    }

//    //clear the entry box
//    tagEntry.value = "";
//    return true;
//}

//function DeleteTag() {
//    let tagCount = 1;
//    let tagList = document.getElementById("TagList");
//    if (!tagList) return false;

//    if (tagList.selectedIndex == -1) {
//        swalWithDarkButton.fire({
//            html: `<span class='font-weight-bolder'>NO TAGS SELECTED TO DELETE</span>`
//        });
//        return true;
//    }

//    while (tagCount > 0) {
//        if (tagList.selectedIndex >= 0) {
//            tagList.options[tagList.selectedIndex] = null;
//            --tagCount;
//        }
//        else
//            tagCount = 0;
//        index--;
//    }
//}
//$("form").on("submit", function () {
//    $("#TagList option").prop("selected", "selected");
//})

//if (typeof tagValues !== 'undefined' && tagValues != '') {
//    let tagArray = tagValues.split(",");
//    for (let loop = 0; loop < tagArray.length; loop++) {
//        ReplaceTag(tagArray[loop], loop);
//        index++;
//    }
//}

//function ReplaceTag(tag, index) {
//    let newOption = new Option(tag, tag);
//    document.getElementById("TagList").options[index] = newOption;
//}

////search function for duplicate tags on the post & return error if found
//function Search(str) {
//    if (str == "") {
//        return `EMPTY tags are not allowed.`;
//    }

//    var tagsEl = document.getElementById('TagList');
//    if (tagsEl) {
//        let tags = tagsEl.options;
//        //for (let i = 0; i < options.length; i++) {
//        //    if (options[i].value == str) {
//        //        return '#${str} was found to be a DUPLICATE tag for this post. Please enter a new tag.';
//        //    }
//        //}
//        for (let i = 0; i < tags.length; i++) {
//            if (tags[i].value == str) {
//                return `#${str} was found to be a DUPLICATE tag for this post. Please enter a new tag.`;
//            }
//        }
//    }
//}
//
//sweet alert
//const swalWithDarkButton = Swal.mixin({
//    customClass: {
//        confirmButton: `btn btn-danger btn-sm btn-block btn-outline-dark`
//    },
//    imageUrl: '/img/doh.jpg',
//    imageHeight: 200,
//    imageAlt: 'Homer Simpson: saying, "Doh!"',
//    timer: 3000,
//    buttonsStyling: false
//});

//To be fair, I did do the hard work above. As my main focus is C#, rather than JavaScript, I was curious if ChatGPT could optimize my JS code and make it more "Enterprise" level.
//I ran a few tests and it seems to check out.
//Below is a more streamlined version that does the same things as the above code.
//-----------------------------------------------------------------------------------------------//
// Enterprise-Grade JavaScript Code (According to ChatGPT):

// Constants
const SELECTED = "selected";

// Variables
let index = 0;

// Functions
function AddTag() {
    // Use search to detect duplicate tags
    let tagEntry = document.getElementById("TagEntry");
    let searchResult = Search(tagEntry.value);

    if (searchResult !== null) {
        showError(`#${searchResult} was found to be a DUPLICATE tag for this post. Please enter a new tag.`);
    } else {
        // Create a new Select option
        let newOption = new Option(tagEntry.value, tagEntry.value);
        document.getElementById("TagList").options[index++] = newOption;
    }

    // Clear the entry box
    tagEntry.value = "";
    return true;
}

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

function ReplaceTag(tag, position) {
    let newOption = new Option(tag, tag);
    document.getElementById("TagList").options[position] = newOption;
}

function Search(str) {
    if (str === "") {
        return "EMPTY tags are not allowed.";
    }

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

function showError(message) {
    Swal.fire({
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
