// [Begin]: Required modules
var express = require("express");
var path = require("path");
var mysql = require("mysql");
var app = express();

var conn = mysql.createConnection({
    user: 'root',
    password: '12345',
    host: 'localhost',
    database: 'mydb'
})

conn.connect(err => {
    console.log(err, "if null, connection successful");
})

app.listen(3000);
app.use("/", express.static(path.join(process.cwd(), "resources")));
// [End]: Required modules

// [Begin]: user table services
app.get("/login", function (req, res) {
    var phone = req.query.phone;
    var password = req.query.password;

    let sql = 'SELECT * FROM user WHERE phone = ? AND password = ?';
    conn.query(sql, [phone, password], (err, result) => {
        if (err) {
            res.send("Error");
        } else {
            if (result.length > 0) {
                res.send("success");
            } else {
                res.send("failed");
            }
        }
    });
});

app.get("/register", function (req, res) {
    var phone = req.query.phone;
    var password = req.query.password;

    let sql = 'INSERT INTO user (phone, password) VALUES (?, ?)';
    conn.query(sql, [phone, password], (err, result) => {
        if (err) {
            res.send("error");
        } else {
            res.send("success");
        }
    });
});

app.get("/user", function (req, res) {
    var phone = req.query.phone;

    let sql = 'SELECT * FROM user WHERE phone = ?';
    conn.query(sql, [phone], (err, result) => {
        if (err) {
            res.send("Error");
        } else {
            res.send(result);
        }
    });
});

app.get("/setuser", function (req, res) {
    var phone = req.query.phone;
    var nickname = req.query.nickname;
    var address = req.query.address;

    let sql = 'UPDATE user SET nickname = ?, address = ? WHERE phone = ?';
    conn.query(sql, [nickname, address, phone], (err, result) => {
        if (err) {
            res.send("Error");
        } else {
            res.send("success");
        }
    });
});

app.get("/address", function (req, res) {
    var phone = req.query.phone;

    let sql = 'SELECT address FROM user WHERE phone = ?';
    conn.query(sql, [phone], (err, result) => {
        if (err) {
            res.send("Error");
        } else {
            res.send(result);
        }
    });
});
// [End]: user table services

// [Begin]: item table services
app.get("/items", function (req, res) {
    let sql = 'SELECT * FROM item';
    conn.query(sql, (err, result) => {
        if (err) {
            res.send("Error");
        } else {
            res.send(result);
        }
    });
});
// [End]: item table services

// [Begin]: cart table services
app.get("/cart_items", function (req, res) {
    var phone = req.query.phone;

    let sql = 'SELECT * FROM cart WHERE phone = ?';
    conn.query(sql, [phone], (err, result) => {
        if (err) {
            res.send("Error");
        } else {
            res.send(result);
        }
    });
});

app.get("/add_cart", function (req, res) {
    var phone = req.query.phone;
    var iname = req.query.iname;
    var showname = req.query.showname;
    var description = req.query.description;
    var price = req.query.price;
    var ar = req.query.ar;

    let sql = 'INSERT INTO cart (phone, iname, price, description, showname, ar) VALUES (?, ?, ?, ?, ?, ?)';
    console.log(sql);
    conn.query(sql, [phone, iname, price, description, showname, ar], (err, result) => {
        if (err) {
            res.send("Error");
        } else {
            res.send("success");
        }
        console.log(result);
    });
});

app.get("/remove_cart", function (req, res) {
    var phone = req.query.phone;
    var iname = req.query.iname;

    let sql = 'DELETE FROM cart WHERE phone = ? AND iname = ?';
    conn.query(sql, [phone, iname], (err, result) => {
        if (err) {
            res.send("Error");
        } else {
            res.send("success");
        }
    });
});
// [End]: cart table services

// [Begin]: order table services
app.get("/orders", function (req, res) {
    var phone = req.query.phone;

    let sql = 'SELECT * FROM orders WHERE phone = ?';
    conn.query(sql, [phone], (err, result) => {
        if (err) {
            res.send("Error");
        } else {
            res.send(result);
        }
    });
});

app.get("/submit_order", function (req, res) {
    var phone = req.query.phone;
    var list = req.query.list;
    var price = req.query.price;
    var address = req.query.address;

    let sql = 'INSERT INTO orders (phone, list, price, address) VALUES (?, ?, ?, ?)';
    conn.query(sql, [phone, list, price, address], (err, result) => {
        if (err) {
            res.send("Error");
        } else {
            res.send("success");
        }
    });
});
// [End]: order table services