function confirmDelete(uinqId, isTrue) {

    var deleteSpan = "deleteSpan_" + uinqId;
    var confirmDeleteSpan = "confirmDeleteSpan_" + uinqId;
    if (isTrue) {
        $("#" + deleteSpan).hide();
        $("#" + confirmDeleteSpan).show();
    } else {
        $("#" + deleteSpan).show();
        $("#" + confirmDeleteSpan).hide();
       
    }
}