$("#add_tag_button").click(function () {
    var input = $("#add_tag").val();
    if (valid_input(input)) {
        add_to_hidden_tag_list(input);
        add_to_tag_list(input);
        update_chosen();
        clear_input();
    } else {
        console.log("Invalid");
    }
});

function clear_input() {
    $("#add_tag").val("");
}

function update_chosen() {
    var options = $("#tag_list").val();
    $("#tag_list").trigger("chosen:updated");
    $("#tag_list").val(options);
}

function add_to_hidden_tag_list(input) {
    var length = ($("#hidden_tags > input").length);
    var hiddenInput = document.createElement("input");
    hiddenInput.type = "hidden";
    hiddenInput.value = input;
    hiddenInput.name = "AddedTags[" + length + "]";
    $("#hidden_tags").append(hiddenInput);
}

function add_to_tag_list(input) {
    var value = -1 * ($("#hidden_tags > input").length);
    var option = new Option(input, value.toString());
    $("#tag_list").append(option);
}


function valid_input(input) {
    return has_characters(input) && unique(input);
}

function has_characters(input) {
    return input.length > 0;
}

function unique(input) {
    var uniqueValue = true;
    $("#tag_list > option").each(function () {
        if (input === this.innerText) {
            uniqueValue = false;
        }
    });
    return uniqueValue;
}
$(document).ready(update_chosen);