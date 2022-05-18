mergeInto(LibraryManager.library, {
    callLogin: function(objName,funcName){
        console.log(UTF8ToString(objName)+" "+UTF8ToString(funcName));
        login(UTF8ToString(objName),UTF8ToString(funcName));
    },

    approveTxn: function(objName,signName,txnHex){
        approve(UTF8ToString(objName),UTF8ToString(funcName),UTF8ToString(txnHex));
    },

    signThisForMe: function(txnHex,seedHex){
        return signTransaction(UTF8ToString(seedHex),UTF8ToString(txnHex));
    }
});