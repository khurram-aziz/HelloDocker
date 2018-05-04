function foo(req, res) {
    req.log("hello from foo() handler");
    return "foo";
}

function summary(req, res) {
    var a, s, h;

    s = "JS summary\n\n";

    s += "Method: " + req.method + "\n";
    s += "HTTP version: " + req.httpVersion + "\n";
    s += "Host: " + req.headers.host + "\n";
    s += "Remote Address: " + req.remoteAddress + "\n";
    s += "URI: " + req.uri + "\n";

    s += "Headers:\n";
    for (h in req.headers) {
        s += "  header '" + h + "' is '" + req.headers[h] + "'\n";
    }

    s += "Args:\n";
    for (a in req.args) {
        s += "  arg '" + a + "' is '" + req.args[a] + "'\n";
    }

    return s;
}

function baz(req, res) {
    res.headers.foo = 1234;
    res.status = 200;
    res.contentType = "text/plain; charset=utf-8";
    res.contentLength = 15;
    res.sendHeader();
    res.send("nginx\n");
    res.send("java\n");
    res.send("script");

    res.finish();
}
