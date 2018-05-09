function toJSON(data) {
    return JSON.stringify(data);
}

Object.defineProperty( Array.prototype,'has',
{
    value:function(o, flag){
    if (flag === undefined) {
        return this.indexOf(o) !== -1;
    } else {   // only for raw js object
        for(var v in this) {
            if( JSON.stringify(this[v]) === JSON.stringify(o)) return true;
        }
        return false;                       
    }}
    // writable:false,
    // enumerable:false
})