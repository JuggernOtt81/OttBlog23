let index = 0;

function AddTag() {
    var tagEntry = document.getElementById("TagEntry");

    //use search to detact duplicate tags
    let searchResult = Search(tagEntry.value);
    if (searchResult != null) {
        //trigger sweet appropriate sweet alert
        Swal.fire({
            title: 'Error!',
            text: 'why u do dis?!',
            icon: 'error',
            confirmButtonText: 'Ok'
        })
    }
    else {
        //create new Select option
        let newOption = new Option(tagEntry.value, tagEntry.value);
        document.getElementById("TagList").options[index++] = newOption;
    }
    
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
    for (let loop = 0; loop < tagArray.length; loop++) {
        ReplaceTag(tagArray[loop], loop);
        index++;
    }
}

function ReplaceTag(tag, index) {
    let newOption = new Option(tag, tag);
    document.getElementById("TagList").options[index] = newOption;
}

//search function for duplicate tags on the post & return error if found
function Search(str) {
    if (str == "") {
        return 'EMPTY tags are not allowed.';
    }

    var tagsEl = document.getElementById('TagList');
    if (tagsEl) {
        let tags = tagsEl.options;
        for (let i = 0; i < tags.length; i++) {
            if (tags[i].value == str) {
                return '#${str} was found to be a DUPLICATE tag for this post. Please enter a new tag.';
            }
        }

    }

}


//sweet alert
