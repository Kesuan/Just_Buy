var express = require("express");
var path = require("path");
var app = express();
app.listen(3000);
app.use("/", express.static(path.join(process.cwd(), "resources")));

app.get("/uploadData", function (req, res) {
    // uid and pwd are the query parameters
    // var uid = req.query.uid;
    // var pwd = req.query.pwd;
    // console.log("User ID: " + uid);
    // console.log("Password: " + pwd);
    res.send("Data uploaded successfully");
});

// var fs = require("fs");
// app.put("/uploadFile", function (req, res) {
//     var dest = req.query("dest");
//     var fd = fs.openSync(dest, "w");
//     req.on("data", function (data) {
//         fs.writeSync(fd, data, 0, data.length, function () { });
//     });

//     req.on("end", function () {
//         res.send("File uploaded successfully");
//         fs.close(fd, function () { });
//     })
// });