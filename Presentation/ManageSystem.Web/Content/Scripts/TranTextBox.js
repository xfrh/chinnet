function keypresshandle(event) {
    console.log(event);
    if (event.keyCode == 38 || event.keyCode == 40 || event.keyCode == 13) {
        focusNextInput(event["srcElement"], false);
    }
}

function focusNextInput(thisInput, up) {
    var inputs = document.getElementsByClassName("add_project_inp");
    console.log(inputs);
    for (var i = 0; i < inputs.length; i++) {

        // 如果是最后一个，则焦点回到第一个
        if (i == (inputs.length - 1)) {
            inputs[0].focus();
            break;
        } else if (thisInput == inputs[i]) {
            if (!up)
                inputs[i + 1].focus();
            else
                inputs[i - 1].focus();
            break; //不加最后一行eles就直接回到第一个输入框
        }
    }
}
