(function() {
    var e, ba = "function" == typeof Object.defineProperties ? Object.defineProperty : function(a, b, c) {
            a != Array.prototype && a != Object.prototype && (a[b] = c.value)
        },
        ca = "undefined" != typeof window && window === this ? this : "undefined" != typeof global && null != global ? global : this;

    function da(a, b) {
        if (b) {
            var c = ca;
            a = a.split(".");
            for (var d = 0; d < a.length - 1; d++) {
                var f = a[d];
                f in c || (c[f] = {});
                c = c[f]
            }
            a = a[a.length - 1];
            d = c[a];
            b = b(d);
            b != d && null != b && ba(c, a, {
                configurable: !0,
                writable: !0,
                value: b
            })
        }
    }
    da("Array.prototype.fill", function(a) {
        return a ? a : function(a, c, d) {
            var b = this.length || 0;
            0 > c && (c = Math.max(0, b + c));
            if (null == d || d > b) d = b;
            d = Number(d);
            0 > d && (d = Math.max(0, b + d));
            for (c = Number(c || 0); c < d; c++) this[c] = a;
            return this
        }
    });
    da("Object.assign", function(a) {
        return a ? a : function(a, c) {
            for (var b = 1; b < arguments.length; b++) {
                var f = arguments[b];
                if (f)
                    for (var g in f) Object.prototype.hasOwnProperty.call(f, g) && (a[g] = f[g])
            }
            return a
        }
    });

    function ea(a) {
        var b = this;
        this.V = "";
        var c = this;
        this.o = a;
        this.o.Re = this.o.Re || !1;
        this.o.Gd = this.o.Gd || !1;
        this.o.vb = this.o.vb || !1;
        this.o.Ze = this.o.Ze || !1;
        this.Fb = this.o.alt || null;
        this.Jb = this.o.Gf;
        this.xf = Date.now();
        this.Lc = this.Ta = this.ic = this.hc = this.Gb = this.A = this.I = this.B = this.a = null;
        this.ac = !0;
        this.Jc = this.$ = null;
        this.za = 1064;
        this.Hf = 600 / this.za;
        this.Sc = this.Zb = this.gc = !1;
        this.Aa = -1;
        this.kb = this.Hb = this.J = this.mb = this.S = this.ra = this.ya = this.xa = !1;
        this.ke = this.Xd = this.Ib = this.he = this.Oc = this.Nc =
            "";
        this.Ia = this.ob = 0;
        this.jb = this.Od = "";
        this.Mc = !1;
        this.jc = {};
        this.Db = this.nc = this.pc = this.sc = this.ta = !1;
        this.Cc = "";
        this.Qc = this.ea = null;
        this.text = {};
        this.Tc = {};
        this.lc = {};
        var d = !1,
            f = [];
        window.onerror = function(a, b, g) {
            if (!b || !g || d || -1 == b.indexOf("/j/")) return !1;
            d = !0;
            a = "ERR " + window.k2url.game + " " + b + ":" + g + " " + a; - 1 == f.indexOf(a) && (f.push(a), b = Math.floor((Date.now() - c.xf) / 1E3), g = Math.floor(b % 60), fa("(" + Math.floor(b / 60) + ":" + Math.floor(g / 10) % 10 + "" + g % 10 + "s,v:" + window.location.hash.substring(1) + ") " + a));
            return d = !1
        };
        this.me = window.k2lang || (window.navigator.language || "en").split("-")[0];
        this.ie = "http:" == document.location.protocol ? "http:" : "https:";
        this.Nc = document.domain ? document.domain : "pl" == this.me ? "www.kurnik.pl" : "www.playok.com";
        this.Oc = "x.playok.com";
        this.he = window.k2hassets || this.Oc;
        this.ke = document.title || "";
        var g = window.navigator.userAgent || "";
        var k = Math.max(screen.width, screen.height);
        var m = Math.min(screen.width, screen.height);
        k = /\(Macintosh;/.test(g) && 4 * m ==
            3 * k;
        this.G = /\(iPhone|\(iPod|\(iPad/.test(g) || k;
        this.ce = -1 != g.indexOf("Android");
        this.Vb = this.G || this.ce;
        this.Kc = -1 != g.indexOf("Mobile") && -1 == g.indexOf("(iPad");
        this.If = -1 != g.indexOf("(Macintosh") && -1 != g.indexOf("Safari/") && -1 == g.indexOf("Chrome");
        this.Cf = /\(iPhone/.test(g);
        this.Jf = -1 != g.indexOf("Trident/");
        this.J = window.k2app || 0;
        (this.mb = window.k2mobile || this.Kc && window.k2app || 0) && (window.k2beta = 0);
        window.k2beta && !k && (this.S = !0, -1 == window.k2beta.indexOf("/1") && (this.Zb = !0));
        this.oe = !this.S && this.o.af;
        if (this.mb || this.oe || 0 == this.Jb) this.Sc = !0;
        document.domain && (-1 != document.cookie.indexOf("kguest=1") && (this.gc = !0, document.cookie = "kguest=0;path=/"), -1 != (k = document.cookie.indexOf("kroom=")) && (this.Ib = document.cookie.substring(k + 6), -1 != (k = this.Ib.indexOf(";")) && (this.Ib = this.Ib.substring(0, k)), document.cookie = "kroom=;path=/"));
        0 != this.Jb && (this.kb = !0);
        this.J && (this.Hb = this.kb = !0);
        h(document.getElementsByTagName("head")[0], "style", {
            type: "text/css"
        }, window.k2style || "");
        this.a = document.getElementById("appcont") ||
            h(document.body, "div", {});
        q(this.a, "k2base");
        this.S && (q(this.a, "aleg"), q(this.a, "hvok"));
        this.G && !/ OS [5-9]_/.test(g) && q(this.a, "iosfix");
        q(this.a, "devios", this.G);
        q(this.a, "devmob", this.Kc);
        q(this.a, "devtch", this.Vb);
        q(this.a, "h100vh", a.Yf || !this.S && !this.o.af || !1);
        this.a.onselectstart = function() {
            return !1
        };
        this.G && (this.a.ontouchstart = function() {});
        this.I = document.getElementById("prehead") || h(this.a, "div");
        this.I != this.a.firstChild && this.a.insertBefore(this.I, this.a.firstChild);
        q(this.I, "anav");
        q(this.I, "usno");
        u(this.I, {
            zIndex: ha
        });
        this.I.ontouchmove = function() {
            return !1
        };
        this.A = document.getElementById("precont") || h(this.a, "div");
        q(this.A, "acon");
        q(this.A, "bsbb");
        if (this.Zb) {
            u(this.a.parentNode, {
                overflow: "hidden"
            });
            q(this.a, "asizing");
            var l = !1;
            this.ne = h(this.a, "div", {
                className: "szpan"
            }, [h("button", {
                className: "bmax",
                onclick: function() {
                    880 <= c.za - 100 && (c.za -= 100);
                    c.qa();
                    this.blur();
                    return !1
                }
            }, "-"), h("button", {
                className: "bmax",
                onclick: function() {
                    c.za += 100;
                    c.qa();
                    this.blur();
                    return !1
                }
            }, "+"),
                h("button", {
                    className: "bmax",
                    style: {
                        background: "#444",
                        color: "#888"
                    },
                    onclick: function() {
                        u(document.body, {
                            background: l ? "#fff" : "#222"
                        });
                        l = !l
                    }
                }, "\u00b7")
            ])
        }
        "#_=_" == window.location.hash && (window.location.hash = "");
        window.onhashchange = function() {
            ia(c)
        };
        window.onkeydown = function(a) {
            27 == a.keyCode && c.$ && c.ac && v(c)
        };
        if (window.cordova) {
            var n;
            this.G && (n = window.StatusBar) && (n.overlaysWebView && n.overlaysWebView(!1), n.styleBlackOpaque && n.styleBlackOpaque(), n.backgroundColorByHexString && n.backgroundColorByHexString("#000"));
            document.addEventListener("resume", function() {
                c.Bf = Date.now();
                c.vf && ja(c, !0)
            }, !1);
            document.addEventListener("backbutton", function() {
                c.$ && c.ac ? v(c) : navigator.app && navigator.app.backHistory()
            }, !1)
        }
        window.onresize = function() {
            if (b.ob) b.Yd = !0;
            else {
                var a = window.innerWidth,
                    c = window.innerHeight;
                b.qa();
                b.Yd = !1;
                b.ob = setTimeout(function() {
                    b.ob = 0;
                    (b.Yd || a != window.innerWidth || c != window.innerHeight || b.G) && b.qa()
                }, 150)
            }
        };
        !this.S && "hidden" in document && document.addEventListener("visibilitychange", function() {
            b.fe =
                document.hidden ? Date.now() : 0
        }, !1);
        if (!this.o.Re) {
            var p = function(a) {
                c.ec && clearTimeout(c.ec);
                c.ec = setTimeout(function() {
                    c.ec = 0;
                    c.ya && c.send(a ? [ka] : [la], null)
                }, 200)
            };
            this.ec = 0;
            this.Cf ? document.addEventListener("visibilitychange", function() {
                b.ya && b.send(document.hidden ? [la] : [ka], null)
            }) : (window.onblur = function() {
                return p(!1)
            }, window.onfocus = function() {
                return p(!0)
            })
        }
        window.onbeforeunload = function() {
            b.Gc = !0
        };
        this.gc && this.o.Gd ? ma(this) : this.Jc = new na({
            host: this.Oc,
            ports: window.k2hcons || ["wss:17003", "wss:443",
                "https:443"
            ],
            pd: function(a) {
                b.Gc || (a == oa || a == pa ? !b.xa && b.o.Gd ? ma(b) : (b.ya = !1, b.xa && b.Ne(), b.vf = Date.now(), a == pa && b.Bf > Date.now() - 2E3 ? ja(b, !0) : qa(b, {
                    connect: !0
                })) : a == ra && qa(b, {
                    Nf: !0
                }))
            },
            ze: function() {
                return !(b.fe && 6E4 < Date.now() - b.fe)
            },
            ye: function(a, c) {
                var d = (window.ap ? "|" + window.ap : "") + (window.ge ? "|" + window.ge : ""),
                    f = screen.width,
                    g = screen.height;
                return {
                    K: [b.o.Tf],
                    O: [sa(b) + d, b.me, window.k2beta ? "b" : b.mb ? "m" : "", b.Ib, window.navigator.userAgent || "", "/" + c + "/" + a, b.J ? "" : "w", (b.ce && g <= f ? g + "x" + f : f + "x" + g) + " " +
                    Math.round(100 * ta()) / 100, "ref:" + window.location.href, "ver:" + window.k2ver + (b.J ? "/app" : "")
                    ]
                }
            },
            tf: function(a, c) {
                return b.Hc(a, c)
            }
        })
    }

    function w(a, b, c) {
        a.lc[b] || (a.lc[b] = []);
        a.lc[b].push(c)
    }

    function y(a, b, c) {
        (a.lc[b] || []).forEach(function(a) {
            "function" === typeof a ? a(c) : a.Vf(c)
        }, a)
    }

    function ua(a) {
        a.Ta = h(a.a, "div", {
            className: "noth",
            style: {
                display: "none",
                zIndex: va,
                position: "fixed",
                top: 0,
                left: 0,
                right: 0,
                bottom: 0,
                width: "100%",
                height: "100%",
                background: "rgba(0,0,0,0.4)"
            },
            onclick: function(b) {
                if (!a.ac) return !1;
                if (b.target != a.Ta && b.target != a.Lc) return !0;
                v(a);
                return !1
            }
        }, a.Lc = h("div", {
            style: {
                display: "table-cell",
                verticalAlign: "middle",
                textAlign: "center"
            }
        }))
    }

    function z(a, b, c, d) {
        a.Ta || ua(a);
        var f = {};
        f.C = h(a.Lc, "div", {
            className: "bsbb bs",
            style: {
                display: "none",
                background: "#fff",
                textAlign: "left",
                minWidth: "260px",
                padding: ".3em 0"
            }
        });
        c && u(f.C, c);
        d && d.noclose || (f.jg = h(f.C, "button", {
            onclick: function() {
                return v(a)
            },
            style: {
                cssFloat: "right",
                width: "3.4em",
                height: "3.4em",
                margin: 0,
                padding: 0,
                color: "#ccc",
                background: "transparent",
                border: "none",
                fontWeight: "normal",
                cursor: "pointer"
            }
        }, "X"));
        f.title = h(f.C, "p", {
            className: "fb",
            style: {
                padding: "0 15px"
            }
        }, [b]);
        f.Ca = h(f.C, "div",
            d && d.nopad ? {} : {
                style: {
                    padding: "0 15px"
                }
            });
        return f
    }

    function E(a, b, c, d) {
        a.$ || (a.ac = !d || !d.nocancel, "undefined" != typeof c && null !== c && b.title && F(b.title, [c]), u(b.C, {
            display: "inline-block"
        }), q(a.Ta, "usno", !d || !d.okselect), u(a.Ta, {
            display: "table"
        }), a.$ = b)
    }

    function v(a) {
        a.$ && (u(a.Ta, {
            display: "none"
        }), u(a.$.C, {
            display: "none"
        }), a.$ = null)
    }

    function sa(a) {
        if (a.gc && !a.kb) return "guest";
        var b = null,
            c = null;
        if (a.kb) try {
            c = window.localStorage.getItem("autoid"), b = window.localStorage.getItem("ksession")
        } catch (k) {}
        if (!b && document.domain)
            for (var d = (document.cookie || "").split(";"), f = 0; f < d.length; f++) {
                var g = d[f];
                " " == g[0] && (g = g.substring(1));
                if ("ksession=" == g.substring(0, 9)) {
                    b = g.substring(9).split(":")[0];
                    break
                }
            }
        return (b || "") + (a.kb ? c && "+" == c[0] ? c.toString() : "+" : "")
    }

    function wa(a) {
        var b = null;
        try {
            b = window.localStorage.getItem("ksession")
        } catch (c) {}
        if (a.kb && b) {
            try {
                window.localStorage.removeItem("ksession")
            } catch (c) {}
            ja(a, !0)
        } else document.body.innerHTML = "", window.location = I(a, "logout") + "?t=" + window.k2url.game
    }

    function ja(a, b) {
        b && (window.onhashchange = null, window.location.hash = a.V);
        document.body.innerHTML = "";
        a.ee && window.AdMob.destroyBannerView && window.AdMob.destroyBannerView();
        window.location.reload()
    }

    function qa(a, b) {
        a.Fc || xa(a, "status");
        F(a.Fc.b, h("table", h("td", [b.Nf ? h("p", h("div", {
            className: "loader"
        })) : null, b.Fd ? h("p", {
            className: "fb"
        }, b.Fd) : null, b.connect ? h("p", h("button", {
            className: "minwd ttup",
            onclick: function() {
                ja(a)
            }
        }, a.g("t_recn", "connect"))) : null, b.link ? h("p", {
            className: "ttup"
        }, h("a", b.link, b.Ff || "-")) : null])));
        ia(a)
    }
    e = ea.prototype;
    e.Hc = function(a, b) {
        switch (a[0]) {
            case ya:
                for (a = 0; a + 1 < b.length; a += 2) this.Pd(b[a], b[a + 1]);
                this.ya = !0;
                ia(this);
                break;
            case za:
                if (1 > b.length) break;
                b = b[0];
                this.Cc = "";
                b = b.split("&");
                for (a = 0; a < b.length; a++) {
                    var c = b[a],
                        d = c.indexOf("=");
                    if (-1 != d) {
                        var f = c.substr(0, d);
                        c = c.substr(d + 1).replace(/%3F/g, "&");
                        d = "1" == c || "true" == c;
                        "noi" == f ? this.sc = d : "nop" == f ? this.pc = d : "prb" == f ? this.nc = d : "snd" == f ? this.ta = d : null != this.Fb && this.Fb == f ? this.Db = d : 0 < f.length && "_" == f.charAt(0) && (this.Cc += "&" + f + "=" + c)
                    }
                }
                break;
            case Aa:
                this.text = {};
                for (a = 0; a < b.length; a += 2) this.text[b[a]] = b[a + 1];
                this.text.t_sf = "pl" != this.lang ? "Send feedback (English)" : "Prze\u015blij uwagi:";
                this.text.t_fb = "pl" != this.lang ? "feedback" : "uwagi";
                (b = this.text.gname) && 0 < b.length && (this.text.gname = b.toUpperCase());
                this.xa || (this.Xc(), this.xa = !0);
                break;
            case Ba:
                if (1 > b.length) break;
                b = {
                    Fd: b[0],
                    link: 3 <= b.length ? {
                        href: b[1],
                        target: "_blank"
                    } : null,
                    Ff: 3 <= b.length ? b[2] : null
                };
                this.ya = !1;
                this.Gc = !0;
                qa(this, b);
                break;
            case Ca:
                if (2 > a.length || !this.J) break;
                try {
                    window.localStorage.setItem("k2ver",
                        a[1]), window.location.reload()
                } catch (g) {
                    this.ya = !1, this.Gc = !0, qa(this, {
                        Fd: "VERSION"
                    })
                }
                break;
            case Da:
                if (!(2 > a.length || 2 > b.length)) {
                    this.Ia = a[1];
                    2 < a.length && (this.Mc = 0 != a[2]);
                    this.Od = b[0];
                    this.lang = b[1];
                    this.Xd = "pl" == this.lang ? "KURNIK" : "PlayOK";
                    if (2 < b.length && 0 < b[2].length) try {
                        window.localStorage.setItem("autoid", b[2])
                    } catch (g) {}
                    3 < b.length && (this.jb = b[3]);
                    for (a = 4; a < b.length; a++) f = b[a].split(":", 2), 1 < f.length && !this.jc.hasOwnProperty(f[0]) && (this.jc[f[0]] = f[1])
                }
        }
    };
    e.g = function(a, b) {
        return this.text[a] || window.k2text && window.k2text[a] || b || a
    };

    function I(a, b) {
        var c = "pl" == a.lang,
            d = a.ie + "//" + a.Nc,
            f = "/" + a.lang,
            g = "&r=" + Date.now();
        switch (b) {
            case "login":
                return d + (c ? "/login.phtml" : f + "/login.phtml") + "?js=1" + g;
            case "register":
                return d + (c ? "/rejestracja.phtml" : f + "/register.phtml") + "?js=1" + g;
            case "profile":
                return d + (c ? "/prof.phtml" : f + "/prof.phtml") + "?js" + g;
            case "tourns":
                return d + (c ? "/turnieje/" : f + "/tournaments/") + "?js&on=" + a.jb + g;
            case "newtourn":
                return d + (c ? "/turnieje/nowy.phtml" : f + "/tournaments/new.phtml") + "?js&gid=" + a.jb + g;
            case "feedback":
                return d +
                    (c ? "/uwagi/" : f + "/feedback/");
            case "passwd":
                return d + (c ? "/haslo.phtml" : f + "/pass.phtml");
            case "logout":
                return d + (c ? "/logout.phtml" : f + "/logout.phtml");
            case "stat":
                return d + (c ? "/stat.phtml?u=%s&g=" + a.jb : f + "/stat.phtml?u=%s&g=" + a.jb)
        }
        return ""
    }
    e.send = function(a, b) {
        this.Jc && this.Jc.send(a, b)
    };
    e.Y = function(a) {
        a.length > Ea && (a = a.substring(0, Ea) + "...");
        this.send([Fa], [a])
    };
    e.rd = function() {
        return this.Db
    };

    function Ga(a) {
        a.send([Ha], ["noi=" + (a.sc ? "1" : "0") + "&nop=" + (a.pc ? "1" : "0") + "&prb=" + (a.nc ? "1" : "0") + "&snd=" + (a.ta ? "1" : "0") + (a.Fb ? "&" + a.Fb + "=" + (a.Db ? "1" : "0") : "") + a.Cc])
    }
    e.Pd = function(a, b) {
        "set_langsymbols" == a && (this.Qc = b.split(" ").map(function(a) {
            return "(" + a + ")"
        }))
    };
    e.Ne = function() {};

    function ma(a) {
        a.xa || (a.Xc(), a.xa = !0);
        ia(a)
    }
    e.Xc = function() {};

    function Ia(a) {
        return 0 != a ? "#" + a : ""
    }

    function Ja(a, b) {
        if (a.o.eg) return b.toString();
        if (0 > b) return "";
        a = Math.floor(b / 1E3);
        return 0 == a && 0 == b % 1E3 ? "?" : Math.floor(5 * a / 10) + "." + 5 * a % 10
    }

    function J(a, b) {
        if (0 == b) return "-";
        if (!a.o.Ze) return b + "";
        a = b - 15E3;
        b = 0 > a ? -a : a;
        return (0 > a ? "\u2013" : "+") + Math.floor(b / 100) + "." + Math.floor(b / 10) % 10 + b % 10
    }

    function Ka(a, b) {
        return a.ea ? 1 + a.ea[b] : 0
    }

    function La(a, b) {
        if (!a.ea) return 0;
        for (var c = 0; c + 1 < a.ea.length && !(b <= a.ea[c]);) c++;
        return c
    }

    function L(a, b, c) {
        var d = window.location.hash.toString();
        "-1" == b ? window.history.back() : b != d.substring(1) && (window.location.hash = b);
        c && ia(a)
    }

    function Ma() {
        var a = window.location.hash.substring(1),
            b; - 1 != (b = a.indexOf("/")) && (a = a.substring(0, b));
        return a
    }

    function Na() {
        var a = window.location.hash.substring(1),
            b;
        return -1 != (b = a.indexOf("/")) ? a.substring(b + 1) : null
    }
    e.Me = function() {
        return "h"
    };
    e.Bd = function(a) {
        return "status" == a ? this.Fc = new Oa(this) : null
    };

    function xa(a, b) {
        a.Tc[b] || (a.Tc[b] = a.Bd(b));
        return a.Tc[b]
    }
    e.Ic = function() {};
    e.qa = function() {
        this.ob && (clearTimeout(this.ob), this.ob = 0);
        var a = window.innerWidth,
            b = window.innerHeight;
        a = this.Sc ? 500 >= a ? 0 : 860 > a ? 1 : 2 : 2;
        var c = !1;
        this.Aa != a && (q(this.a.parentNode, "vm" + this.Aa, !1), this.Aa = a, q(this.a.parentNode, "vm" + this.Aa), this.ra = 2 <= a, c = !0);
        if (this.Zb) {
            c = this.za;
            var d = Math.round(c * this.Hf),
                f = Math.round(.95 * d),
                g = b;
            d <= b - 48 ? g = d : f <= b - 48 && (g = b - 48);
            2 > a && (g = b);
            a = Math.min(Math.floor((b - g) / 2), 36);
            this.te = {
                Uf: c,
                Af: Math.floor(g),
                zf: b - a,
                top: a
            };
            this.ne && 0 < a && u(this.ne, {
                top: -(a >> 1) + "px"
            });
            c = !0
        }
        c ?
            ia(this) : this.Ic(!1)
    };

    function ia(a) {
        if (-1 == a.Aa) setTimeout(function() {
            return a.qa()
        }, 0);
        else {
            var b = a.Me(Ma()),
                c = xa(a, a.ya ? b : "status");
            if (c) {
                var d = c.b,
                    f = a.B == d;
                "function" === typeof c.Pb && c.Pb(!f);
                a.B && a.B != d && u(a.B, {
                    display: "none"
                });
                if (a.Zb) {
                    var g = a.te;
                    u(a.a, {
                        maxWidth: g.Uf + "px",
                        height: (c.Vd ? g.Af : g.zf) + "px",
                        minHeight: 0,
                        top: g.top + "px"
                    });
                    q(a.a, "dosize", c.Sf && 0 < g.top || !1)
                }
                q(a.a, "donav", !c.Vd);
                "string" == typeof a.o.yf && q(a.a, a.o.yf, !!c.Rf);
                c.Ud || q(a.a, "doddmenu", "undefined" === typeof c.Ud ? !a.S || 2 > a.Aa : !1);
                g = 2 <= a.Aa && a.S ? a.ic :
                    "function" === typeof c.gf ? c.gf() : null;
                var k = !1;
                !g && a.hc && c.Oa && (g = a.hc, a.hc.innerHTML = c.Oa, k = !0);
                a.Gb && a.Gb != g && u(a.Gb, {
                    display: "none"
                });
                a.Gb != g && (a.Gb = g) && u(g, {
                    display: k ? "block" : "inline-block"
                });
                a.B = d;
                y(a, "nav", b);
                if (!f && (u(d, {
                    display: "block"
                }), a.ra || ((document.scrollingElement || document.documentElement).scrollTop = 0), "function" === typeof c.onshow)) c.onshow();
                a.Ic(!f)
            }
        }
    }

    function Pa(a) {
        ea.call(this, a);
        this.lf = "f";
        this.qf = "p";
        this.rf = "i";
        this.Wd = "t";
        this.mf = "n";
        this.P = this.ja = this.j = this.tab = null;
        this.v = {};
        this.Mb = [];
        this.f = {};
        this.L = {};
        this.sa = {};
        this.nb = [];
        this.le = [];
        this.be = this.de = this.je = null;
        this.pe = this.N = this.yc = this.Ac = this.Ha = this.uc = 0;
        this.Xb = null;
        this.Wb = 0
    }
    Pa.prototype = Object.create(ea.prototype);
    e = Pa.prototype;
    e.constructor = Pa;
    e.Xc = function() {
        function a() {
            return [h("b", d.Od), d.o.vb ? null : h("div", {
                className: "mlh r" + La(d, d.Ha)
            }), d.o.vb ? null : h("span", {
                className: "snum"
            }, J(d, d.Ha))]
        }

        function b(a) {
            return [h("div", {
                className: "spbb"
            }), " (" + a + ")"]
        }

        function c(a) {
            return [h("div", {
                className: "spbb"
            }), " (" + a + ")"]
        }
        var d = this,
            f = this;
        this.ic = h(this.I, "div", {
            className: "dclpd bsbb navcont",
            style: {
                display: "none"
            }
        });
        var g, k, m, l;
        h(this.ic, "div", {
            className: "newtab1"
        }, [g = h("button", {
                className: "butsys minwd",
                onclick: function() {
                    f.send([Qa], null)
                }
            },
            this.g("bl_newtab")), " ", h("div", {
            className: "selcwr mro"
        }, [k = h("button", {
            className: "selcbt butsys vsel"
        }, ["-"]), m = h("select", {
            className: "selcsl",
            onchange: function(a) {
                (a = a.target.options[a.target.selectedIndex]) && f.Y("/join " + a.text.split(" ")[0])
            }
        })]), l = h("span", {
            className: "tuinfo fb"
        }, "-")]);
        w(this, "rooms", function(a) {
            m.options.length = 0;
            h(m, a.list.map(function(b, c) {
                return h("option", c == a.Ab ? {
                    selected: !0
                } : {}, b)
            }));
            F(k, [a.list[0 <= a.Ab ? a.Ab : 0].split("(")[0] || "-"])
        });
        w(this, "tumode", function(a) {
            g.disabled = !!a
        });
        w(this, "tuinfo", function(a) {
            return F(l, [a || "-"])
        });
        var n = h(this.ic, "div", {
                className: "nav ib"
            }),
            p = {};
        p.h = h(n, "button", {
            className: "bmain",
            onclick: function() {
                L(f, f.V)
            }
        }, this.g("bl_tbs"));
        if (0 == this.Ia) {
            var t = p.c = h(n, "button", {
                onclick: function() {
                    L(f, "c")
                }
            }, [f.g("bl_buds") + " (0)"]);
            var r = p.m = h(n, "button", {
                onclick: function() {
                    L(f, "m")
                }
            }, b(0));
            this.Mc && (p.t = h(n, "button", {
                onclick: function() {
                    L(f, "t")
                }
            }, this.g("t_turs")))
        }
        p.e = h(n, "button", {
            onclick: function() {
                L(f, "e")
            }
        }, this.g("bl_mr"));
        var B = p.x = h(n, "button", {
            className: "btab",
            onclick: function() {
                0 < f.tab.F && L(f, f.tab.F.toString())
            }
        }, "#000");
        w(this, "tabalert", function(a) {
            return q(B, "alert", a)
        });
        w(this, "tabopen", function(a) {
            return F(B, [0 < a ? "#" + a : "#000"])
        });
        t && w(this, "ncontacts", function(a) {
            return F(t, [f.g("bl_buds") + " (" + a + ")"])
        });
        r && w(this, "nmessages", function(a) {
            F(r, b(a));
            q(r, "alert", 0 < a)
        });
        w(this, "nav", function(a) {
            Object.keys(p).forEach(function(b) {
                return q(p[b], "active", b == a)
            })
        });
        this.hc = h(this.I, "div", {
            className: "navttl fb fl",
            style: {
                display: "none"
            }
        });
        var x = h(this.a, "div", {
                className: "nav0 usno tama",
                style: {
                    zIndex: ha + 1
                }
            }),
            H = h(x, "button", {
                className: "mbut hdhei hdbwd",
                onclick: function() {
                    Ra(x);
                    return !1
                }
            }, [h("div", {
                className: "micon"
            })]);
        document.addEventListener("click", function(a) {
            !M(x, "nav0open") || x.contains(a.target) && !A.contains(a.target) || (Ra(x), A.contains(a.target) || a.stopPropagation())
        }, !0);
        w(this, "chatalert", function(a) {
            return q(H, "alert", a)
        });
        n = h(x, "div", {
            className: "mcont"
        });
        var A = h(n, "div", {
                className: "mlst"
            }),
            G = {};
        var K = G.x = h(A, "button", {
            className: "btab",
            onclick: function() {
                0 < f.tab.F && L(f, f.tab.F.toString())
            }
        }, "#000");
        G.h = h(A, "button", {
            onclick: function() {
                L(f, f.V)
            }
        }, this.g("bl_tbs"));
        if (0 == this.Ia) {
            var C = G.c = h(A, "button", {
                onclick: function() {
                    L(f, "c")
                }
            }, [f.g("bl_buds") + " (0)"]);
            var D = G.m = h(A, "button", {
                onclick: function() {
                    L(f, "m")
                }
            }, c(0));
            this.Mc && (G.t = h(A, "button", {
                onclick: function() {
                    L(f, "t")
                }
            }, this.g("t_turs")))
        } else this.S || (G.l = h(A, "button", {
            onclick: function() {
                f.Hb ? L(f, "l") : wa(f)
            }
        }, this.g("t_lgin")));
        G.e = h(A, "button", {
                onclick: function() {
                    L(f, "e")
                }
            },
            this.g("bl_mr"));
        w(this, "tabalert", function(a) {
            return q(K, "alert", a)
        });
        w(this, "tabopen", function(a) {
            return F(K, [0 < a ? "#" + a : "#000"])
        });
        C && w(this, "ncontacts", function(a) {
            return F(C, [f.g("bl_buds") + " (" + a + ")"])
        });
        D && w(this, "nmessages", function(a) {
            F(D, c(a));
            q(D, "alert", 0 < a)
        });
        w(this, "nav", function(a) {
            Object.keys(G).forEach(function(b) {
                return q(G[b], "active", b == a)
            })
        });
        n = h(n, "div", {
            className: "msub"
        });
        var Cd = h(n, "p", a());
        w(this, "urank", function() {
            return F(Cd, a())
        });
        this.J || this.S || h(n, "p", [h("button", {
            onclick: function() {
                document.body.innerHTML =
                    "";
                window.location = window.k2url.home
            }
        }, this.Xd)]);
        w(this, "tabopen", function(a) {
            0 < a ? setTimeout(function() {
                return q(f.a, "navtabopen", 0 < a)
            }, 0) : q(f.a, "navtabopen", 0 < a)
        });
        w(this, "tumode", function(a) {
            return q(f.a, "tumode", !!a)
        });
        xa(this, "h");
        n = this.jc.hasOwnProperty("admid") && this.jc.admid || 0;
        "dm" == this.jb && (n = "ca-app-pub-9206768950691556/9205709028");
        this.J && window.AdMob && n && this.Kc && 480 < screen.height && (window.AdMob.setOptions({
            publisherId: n,
            bannerAtTop: !1,
            overlap: !1,
            offsetTopBar: !0,
            isTesting: !1,
            autoShow: !1
        }),
            this.Df = 1)
    };
    e.Ne = function() {
        Sa(this.j, [0, 0, 0], []);
        this.N && (clearInterval(this.N), this.N = 0);
        this.tab && this.tab.reset()
    };

    function Ta(a, b) {
        a.Uc || (a.Uc = z(a, "-"));
        F(a.Uc.Ca, h("p", [h("button", {
            className: "minw",
            onclick: function() {
                v(a);
                a.send([Ua, 0], [b]);
                return !1
            }
        }, a.g("tu_bcan")), h("button", {
            className: "minw",
            onclick: function() {
                v(a);
                a.send([Fa], ["/boot " + b]);
                return !1
            }
        }, a.g("bl_boot"))]));
        E(a, a.Uc, b)
    }

    function Va(a, b, c) {
        a.Rc || (a.Rc = z(a, "-"));
        F(a.Rc.Ca, [h("p", [h("button", {
            className: "minw",
            onclick: function() {
                v(a);
                a.send([Wa], [b]);
                return !1
            }
        }, "info"), h("button", {
            className: "minw",
            disabled: 0 != c,
            onclick: function() {
                v(a);
                a.send([Ua, 1], [b]);
                return !1
            }
        }, a.g("tu_bacc"))]), h("p", [h("button", {
            className: "minw",
            disabled: !(0 <= c),
            onclick: function() {
                v(a);
                a.send([Ua, 0], [b]);
                return !1
            }
        }, a.g("tu_bcan")), h("button", {
            className: "minw",
            onclick: function() {
                v(a);
                a.send([Fa], ["/boot " + b]);
                return !1
            }
        }, a.g("bl_boot"))])]);
        E(a, a.Rc,
            b)
    }

    function Xa(a) {
        document.title = a.ke + (a.Xb ? " " + a.Xb : 0 < a.Wb ? " (" + a.Wb + ")" : "")
    }

    function Ya(a, b) {
        a.Wb != b && (a.Wb = b, Xa(a))
    }

    function Za(a) {
        return !!a.tab && a.tab.b == a.B
    }

    function $a(a) {
        L(a, a.tab && a.tab.Pe ? "-1" : a.V)
    }

    function ab(a, b, c) {
        a.send(c ? [bb, b, 1] : [bb, b], null)
    }

    function cb(a, b) {
        return a.v.hasOwnProperty(b) ? a.v[b] : null
    }

    function db(a, b) {
        eb(b, a.v);
        a.Mb.push(b)
    }

    function fb(a, b, c) {
        "set_tournaments" == b ? (b = parseInt(c, 10), b != a.yc && (a.yc = b, y(a, "tumode", a.yc))) : "set_cols" == b ? a.Ac = parseInt(c, 10) : "set_chat" == b && y(a, "chmode", "0" != c)
    }
    e.Pd = function(a, b) {
        ea.prototype.Pd.call(this, a, b);
        "set_tab_mt" == a ? this.je = b.split(" ") : "set_tab_at" == a ? this.be = b.split(" ") : "set_tab_gt" == a ? this.de = b.split(" ") : "set_rank" == a && (this.ea = b.split(" ").filter(function(a, b) {
            return 0 == b % 2
        }).map(function(a) {
            return parseInt(a, 10)
        }))
    };

    function gb(a, b, c, d, f, g) {
        var k = a.j.N,
            m = f && 0 < f.length;
        k && F(k, h("div", {
            className: "alrt dcpd"
        }, [h("div", {
            className: "mbsp"
        }, b), h("button", {
            className: "minw",
            onclick: function() {
                c && a.send(c, d);
                hb(a)
            }
        }, a.g(m ? "bl_yes" : "bl_ok")), m ? h("button", {
            className: "minw",
            onclick: function() {
                a.send(f || [], g);
                hb(a)
            }
        }, a.g("bl_no")) : null]))
    }

    function hb(a) {
        (a = a.j.N) && F(a, [])
    }

    function ib(a) {
        Object.keys(a.v).forEach(function(b) {
            jb(a.v[b]);
            delete a.v[b]
        }, a)
    }

    function kb(a) {
        var b = 0;
        Object.keys(a.L).forEach(function(c) {
            0 != a.L[c].Qa && b++
        }, a);
        y(a, "nmessages", b);
        y(a, "chatalert", 0 < b);
        Ya(a, b)
    }

    function lb(a, b) {
        a.P || xa(a, "m");
        mb(a.P, b);
        L(a, "m" + (a.ra ? "" : "/" + b))
    }
    e.Hc = function(a, b) {
        var c = this,
            d;
        if (a[0] >= nb) 1 < a.length && this.tab && this.tab.F == a[1] && ob(this.tab, a, b);
        else switch (a[0]) {
            case pb:
                if (1 > b.length) break;
                this.nb = [];
                this.sa = {};
                a = b[0].split(" ");
                for (d = 0; d < a.length; d++) {
                    var f = a[d];
                    var g = "_" + f;
                    this.sa.hasOwnProperty(g) || (this.sa[g] = {
                        Ma: f,
                        se: 1
                    }, this.nb.push(f))
                }
                this.le = 1 < b.length && 0 < b[1].length ? b[1].split(" ") : [];
                this.P && qb(this.P);
                break;
            case rb:
                if (2 > b.length) break;
                f = 1 < a.length ? a[1] : 0;
                a = 2 < a.length ? a[2] : sb;
                g = !1;
                var k = b[1],
                    m = "_" + k,
                    l = this.L.hasOwnProperty(m) ? this.L[m] :
                        null;
                if (a == tb || a == sb) {
                    l || (this.L[m] = l = {
                        Ma: k,
                        zc: "",
                        Qa: 0,
                        Bc: 0
                    });
                    l.zc = "" != l.zc ? l.zc + "\n" + b[0] : b[0];
                    if (0 != f) {
                        l.Qa = f;
                        var n = 0,
                            p = b[0].split("\n");
                        for (d = 0; d < p.length; d++) 0 != p[d].indexOf(N) && n++;
                        l.Bc += n
                    }
                    this.sa.hasOwnProperty(m) || (this.sa[m] = {
                        Ma: k,
                        se: 0
                    }, this.nb.unshift(k), g = !0)
                } else a == ub && (l && delete this.L[m], this.sa.hasOwnProperty(m) && (delete this.sa[m], -1 != (d = this.nb.indexOf(k)) && this.nb.splice(d, 1)));
                this.P && vb(this.P, k, a, b[0], f, g);
                this.P || kb(this);
                break;
            case wb:
                if (1 > a.length || !this.P) break;
                for (d = 1; d <
                a.length && !(d - 1 >= b.length); d++) xb(this.P, b[d - 1], a[d]);
                break;
            case yb:
                if (1 > b.length || !this.j) break;
                this.j && zb(this.j, b[0]);
                break;
            case Ab:
                if (2 > a.length) break;
                2 < a.length && (this.tab || xa(this, "x"), Bb(this.tab, a, b));
                L(this, a[1].toString(), !0);
                break;
            case Cb:
                if (2 > a.length) break;
                Ma() == a[1] && $a(this);
                this.tab && this.tab.F == a[1] && this.tab.reset();
                break;
            case Db:
                if (3 > a.length) break;
                Sa(this.j, a, b);
                this.tab || setTimeout(function() {
                    c.tab || xa(c, "x")
                }, 100);
                break;
            case Eb:
                if (1 > b.length || 3 > a.length) break;
                this.j.a.hasOwnProperty(a[1]) ?
                    Fb(this.j, a, b) : Gb(this.j, a, b);
                this.tab && this.tab.F == a[1] && Hb(this.tab, a, b);
                break;
            case Ib:
                if (2 > a.length) break;
                Jb(this.j, a[1]);
                break;
            case Kb:
                if (1 > b.length) break;
                gb(this, b[0]);
                break;
            case Lb:
                if (2 > b.length || 2 > a.length) break;
                gb(this, b[0], [bb, a[1]], null, [Mb, a[1]], [b[1]]);
                break;
            case Nb:
                if (1 > b.length) break;
                this.v.hasOwnProperty(b[0]) && (jb(this.v[b[0]]), delete this.v[b[0]]);
                break;
            case Ob:
                if (1 > b.length || 2 > a.length) break;
                if (this.v.hasOwnProperty(b[0])) Pb(this.v[b[0]], this.Ac, a.slice(1));
                else
                    for (a = new Qb(this,
                        b[0], this.Ac, a.slice(1)), this.v[b[0]] = a, d = this.Mb.length - 1; 0 <= d; d--) Rb(this.Mb[d], a);
                break;
            case Sb:
                if (3 > a.length) break;
                ib(this);
                d = a[1];
                f = a[2];
                g = 3;
                for (k = 0; g + d <= a.length && k + 1 + f <= b.length;) this.v[b[k]] = new Qb(this, b[k], this.Ac, a.slice(g, g + d)), g += d, k += 1 + f;
                for (d = this.Mb.length - 1; 0 <= d; d--) eb(this.Mb[d], this.v);
                break;
            case Tb:
                for (d = 0; d < a.length - 1 && 2 * d < b.length; d++) this.f.hasOwnProperty(b[2 * d]) && (f = this.f[b[2 * d]]) && (f.status = a[1 + d], 2 * d + 1 < b.length && (f.fb = b[2 * d + 1]), Ub(f, this.g("st_st1")), this.ja && Vb(this.ja,
                    f));
                var t = 0;
                Object.keys(this.f).forEach(function(a) {
                    c.f[a].status && t++
                }, this);
                y(this, "ncontacts", t);
                break;
            case Wb:
                if (2 > a.length) break;
                if (0 == a[1]) {
                    Object.keys(this.f).forEach(function(a) {
                        delete c.f[a]
                    }, this);
                    for (d = 0; d < b.length - 1; d += 2) this.f.hasOwnProperty(b[d]) || (f = {
                        name: b[d],
                        status: 0,
                        fb: b[d + 1]
                    }, Ub(f, this.g("st_st1")), this.f[b[d]] = f);
                    this.ja && Xb(this.ja, this.f)
                } else if (1 == a[1]) {
                    if (3 > a.length || 2 > b.length) break;
                    if (this.f.hasOwnProperty(b[0])) break;
                    f = {
                        name: b[0],
                        status: a[2],
                        fb: b[1]
                    };
                    Ub(f, this.g("st_st1"));
                    this.f[b[0]] = f;
                    this.ja && Yb(this.ja, f)
                } else if (-1 == a[1]) {
                    if (1 > b.length) break;
                    if (!this.f.hasOwnProperty(b[0])) break;
                    f = this.f[b[0]];
                    if (!f) break;
                    delete this.f[b[0]];
                    this.ja && Zb(this.ja, f)
                }
                var r = 0;
                Object.keys(this.f).forEach(function(a) {
                    c.f[a].status && r++
                }, this);
                y(this, "ncontacts", r);
                break;
            case $b:
                break;
            case ac:
                if (1 > b.length || !this.j) break;
                d = 0;
                1 < a.length && (d = a[1]);
                this.N && (clearInterval(this.N), this.N = 0);
                if (0 > d) {
                    var B = function(a) {
                        y(c, "tuinfo", 0 > a ? "(?)" : Math.floor(a / 60) + ":" + Math.floor(a % 60 / 10) % 10 + a % 60 %
                            10)
                    };
                    B(-d);
                    this.pe = Date.now() + 1E3 * -d + 100;
                    this.N = setInterval(function() {
                        var a = Math.floor((c.pe - Date.now()) / 1E3);
                        B(a);
                        0 > a && c.N && (clearInterval(c.N), c.N = 0)
                    }, 1E3)
                }
                0 <= d && y(this, "tuinfo", b[0]);
                break;
            case bc:
                cc(this.j);
                ib(this);
                d = 0;
                for (a = b.length; d + 1 < a; d += 2) fb(this, b[d], b[d + 1]);
                break;
            case dc:
                if (2 > a.length || 1 > b.length) break;
                b = b[0].split("\n");
                this.yc && b.unshift("-");
                y(this, "rooms", {
                    list: b,
                    Ab: a[1]
                });
                this.Ef ? (hb(this), L(this, this.V)) : this.Ef = !0;
                break;
            case ec:
                if (2 > a.length || 1 > b.length) break;
                1 == b.length ? gb(this,
                    b[0], [fc, 1], null, [fc, 0], null) : gb(this, b[0], [fc, 2], [b[1]], [fc, 0], [b[1]]);
                break;
            case gc:
                if (2 > a.length) break;
                if (this.uc == a[1]) break;
                this.uc = a[1];
                y(this, "urole", this.uc);
                break;
            case hc:
                if (2 > a.length) break;
                this.Ha = a[1];
                y(this, "urank", this.Ha);
                break;
            case ic:
                if (1 > b.length) break;
                jc(this, b[0], a, b);
                break;
            default:
                ea.prototype.Hc.call(this, a, b)
        }
    };

    function jc(a, b, c, d) {
        if (!(2 > c.length)) {
            a.Dc || (a.Dc = z(a, "-"), a.Dc.C.onselectstart = function(a) {
                a.stopPropagation();
                return !0
            });
            for (var f = 5 + 2 * c[4], g = 1 + c[4], k, m = [], l = 0; l < Math.min(c[4], 2); l++) {
                var n = (0 > c[5 + 2 * l] ? 65536 + c[5 + 2 * l] : c[5 + 2 * l]) + 65536 * c[2 * l + 6];
                m.push(d[1 + l] + " " + (0 < l ? n.toString() : J(a, n)));
                0 == l && (k = n)
            }
            var p = n = "",
                t = ["-", "", "", ""];
            for (l = 0; l < c.length - f; l++) {
                var r = d[g + l];
                switch (c[f + l]) {
                    case 0:
                        t[0] = r;
                        break;
                    case 2:
                        t[1] = r;
                        break;
                    case 3:
                        t[2] = r;
                        break;
                    case 4:
                        t[3] = r;
                        break;
                    case 5:
                        n = r;
                        break;
                    case 6:
                        p = r
                }
            }
            d = p || "photos/none.jpg";
            f = t[0] + ("" != t[1] && "-" != t[1] ? " (" + t[1] + ")" : "");
            t = t[2] + ("" != t[3] ? ("" != t[2] ? ", " : "") + t[3] : "");
            var B = 0 != (c[1] & kc),
                x = 0 != (c[1] & lc),
                H = c[2],
                A, G;
            F(a.Dc.Ca, ["undefined" != typeof k ? h("p", {
                className: "fb mbh",
                style: {
                    marginTop: "-0.25em"
                }
            }, [m[0], " ", h("div", {
                className: "r" + La(a, k)
            }), h("br"), m[1]]) : null, c = h("p", {
                style: {
                    margin: ".5em 0",
                    width: "220px",
                    padding: ".2em 0",
                    wordWrap: "break-word",
                    overflowY: "auto"
                }
            }, [h("div", {
                style: {
                    cssFloat: "left",
                    marginTop: "3px",
                    marginRight: ".5em",
                    width: "52px",
                    height: "52px",
                    border: "solid 1px #aaa",
                    overflow: "hidden"
                }
            }, h("img", {
                src: a.ie + "//" + a.he + (d && "/" != d[0] ? "/" : "") + d
            })), h("div", f), h("div", t), h("div", n)]), a.J ? null : h("p", {
                className: "mbh"
            }, h("a", {
                className: "lbut minwd",
                target: "_blank",
                href: mc(I(a, "stat"), encodeURIComponent(b)),
                onclick: function() {
                    v(a)
                }
            }, a.g("ui_stats"))), h("p", {
                className: "mbh"
            }, [h("button", {
                className: "minw",
                disabled: 0 == H,
                onclick: function() {
                    v(a);
                    ab(a, H)
                }
            }, 0 < H ? "#" + H : "#000"), 0 == a.Ia ? h("a", {
                href: "",
                title: a.g("bl_whisper"),
                className: "spbb",
                onclick: function() {
                    v(a);
                    lb(a, b);
                    return !1
                },
                style: {
                    margin: "0 1em"
                }
            }) : null]), h("div", {
                className: "dtline"
            }), 0 == a.Ia ? h("p", [A = h("input", {
                type: "checkbox",
                checked: B
            }), a.g("bl_buds"), " ", G = h("input", {
                type: "checkbox",
                checked: x
            }), a.g("ui_block")]) : null, h("p", h("button", {
                className: "minw",
                onclick: function() {
                    v(a);
                    A && A.checked != B && a.Y((A.checked ? "/buddy" : "/unbuddy") + " " + b);
                    G && G.checked != x && a.Y((G.checked ? "/ignore" : "/unignore") + " " + b);
                    return !1
                }
            }, a.g("bl_ok")))]);
            nc(c);
            E(a, a.Dc, b, {
                okselect: !0
            })
        }
    }
    e.Ic = function() {
        if (Za(this)) {
            var a = this.tab;
            a.Ed = a.app.mb || a.app.oe;
            a.Ed ? (M(a.b, "sbfixed") && q(a.b, "sbfixed", !1), q(a.b, "gvhead", !0), q(a.b, "sbdrop")) : (M(a.b, "sbdrop") && q(a.b, "sbdrop", !1), q(a.b, "gvhead", !1), q(a.b, "sbfixed"));
            q(a.app.a, "doddmenu", a.Ed);
            a.u.qa()
        }!this.Df || Za(this) || this.ee || (window.AdMob.createBannerView(), this.ee = 1);
        if (!this.S && !this.Sc && 1 < this.Jb && this.xa) {
            a = document.body.offsetWidth;
            var b = document.body.offsetHeight;
            300 <= a && b > 1.1 * a ? (this.tc || (this.tc = z(this, this.g("t_rot"), {
                textTransform: "uppercase",
                textAlign: "center"
            }, {
                noclose: !0
            }), h(this.tc.Ca, "p", {
                className: "ttup"
            }, this.g("t_rot"))), E(this, this.tc, null, {
                nocancel: !0
            })) : this.$ && this.$ == this.tc && v(this)
        }
    };
    e.Me = function(a) {
        var b = parseInt(a, 10);
        return !isNaN(b) && 0 < b ? "x" : "f" == a || "p" == a || "c" == a || "m" == a || "e" == a || 0 == this.Ia && "i" == a || 0 == this.Ia && ("t" == a || "n" == a) || this.Hb && ("l" == a || "s" == a) ? a : "h"
    };
    e.Bd = function(a) {
        var b = this;
        return "h" == a ? this.j = new oc(this, this.Jb) : "x" == a ? (this.tab = new(this.o.Qf || Object.constructor)(this), this.tab.ha(this), this.tab) : "c" == a ? this.ja = new pc(this) : "m" == a ? this.P = new qc(this) : "e" == a ? new rc(this) : "f" == a ? new sc(this) : "p" == a ? new tc(this) : "l" == a && this.Hb ? new O(this, {
            url: I(this, "login"),
            pb: "k2qlog",
            Cb: this.g("t_lgin") + " (" + this.Xd + ")",
            tb: function(a) {
                "v_signup" == a && L(b, "s")
            }
        }) : "s" == a && this.Hb ? new O(this, {
            url: I(this, "register"),
            pb: "k2qreg",
            Cb: this.g("t_hdrg")
        }) : "i" == a ?
            new O(this, {
                url: I(this, "profile"),
                pb: "k2qprof",
                Cd: !0,
                Cb: this.g("t_prof")
            }) : "t" == a ? new uc(this, {
                url: I(this, "tourns"),
                pb: "k2qtl",
                Cb: this.g("t_turs")
            }) : "n" == a ? new O(this, {
                url: I(this, "newtourn"),
                pb: "k2qnt",
                Cd: !0,
                Cb: this.g("t_turs")
            }) : ea.prototype.Bd.call(this, a)
    };

    function vc() {
        var a = {
                Of: wc
            },
            b = document.getElementById("appcont") || h(document.body, "div", {
                id: "appcont"
            });
        a = new(a.Of || Object.constructor)(a.hg || {});
        F(b, a.b);
        a.start()
    }

    function xc(a) {
        var b = this;
        this.fd = a;
        this.app = {
            rd: function() {
                return 0
            },
            te: {
                top: 1
            }
        };
        var c = window.navigator.userAgent || "",
            d = /\(iPhone|\(iPod|\(iPad/.test(c);
        c = -1 != c.indexOf("Android");
        this.app.Vb = d || c;
        this.app.S = !this.app.Vb;
        this.a = [];
        this.ia = 0;
        this.f = -1;
        this.v = "";
        this.A = !1;
        this.j = [];
        h(document.getElementsByTagName("head")[0], "style", {
            type: "text/css"
        }, "button { font: inherit; background: none; border-width: 1px; padding: .3em; outline: 0; } button { touch-action: manipulation; } a { text-decoration: none; color: inherit; } .usno { user-select: none; -webkit-user-select: none; }.noth { -webkit-tap-highlight-color: rgba(0,0,0,0); }.bsbb { box-sizing: border-box; } .bcont { position: absolute; left: 0; top: 0; width: 100%; height: 100%; } .rbcont { position: relative; }\t.rbratio { padding-top: 95%; } .rmcont { min-height: 5em; } .rmcont { word-wrap: break-word; } .rmcont span { cursor: pointer; } @media (min-width: 570px) { \t.rbcont { margin-right: 32%; }\t.rbratio { padding-top: 82%; } \t.rmcont { padding: 0 10px; overflow-y: auto; position: absolute; width: 32%; right: 0; top: 0; bottom: 0; box-sizing: border-box; }   .rmcbg { background: #f4f4f4; }  .rmcfr { border: solid 1px #ddd; }}");
        var f = this;
        this.b = h("div", {
            tabIndex: "0",
            style: {
                position: "relative",
                outline: 0
            }
        }, [d = h("div", {
            className: "rbcont"
        }, [h("div", {
            className: "rbratio"
        })]), h("div", {
            className: "rmcont " + (a.Xf ? "rmcfr" : "rmcbg")
        }, [h("p", [h("button", {
            onclick: function() {
                0 < f.ia && f.u.da(f.ia - 1)
            },
            style: {
                minWidth: "4em"
            }
        }, "<"), " ", h("button", {
            onclick: function() {
                f.ia < f.a.length - 1 && f.u.da(f.ia + 1)
            },
            style: {
                minWidth: "4em"
            }
        }, ">"), " ", this.B = h("span", {
            style: {
                whiteSpace: "nowrap"
            }
        })]), this.J = h("p", ["-"]), this.G = h("p", {}), this.I = h("p", {})])]);
        (window !=
        window.top ? this.b : window).addEventListener("keydown", function(a) {
            switch (a.keyCode) {
                case 37:
                case 38:
                    0 < b.ia && b.u.da(b.ia - 1);
                    break;
                case 39:
                case 40:
                    b.ia < b.a.length - 1 && b.u.da(b.ia + 1);
                    break;
                case 35:
                    b.u.da(b.a.length - 1);
                    break;
                case 36:
                    b.u.da(0);
                    break;
                default:
                    return
            }
            a.preventDefault()
        }, !1);
        this.u = new(a.nf || Object.constructor);
        this.u.ha(this);
        d.appendChild(this.u.b);
        this.fd.fg && this.u.setActive(!0);
        "undefined" != typeof this.u.bb && h(d, "a", {
            onclick: function() {
                f.u.yd(f.u.bb ? 0 : 1)
            },
            style: {
                cursor: "pointer",
                color: "rgba(0,0,0,0.25)",
                position: "absolute",
                left: 0,
                top: 0,
                padding: "2px 8px",
                zIndex: 1E3
            }
        }, "\u25be");
        window.onresize = function() {
            return b.qa()
        };
        window.onhashchange = function() {
            var a = yc();
            a.wd != b.f ? zc(b, a.wd, a.dc) : a.dc != b.ia && b.u.da(a.dc)
        }
    }
    e = xc.prototype;
    e.Te = function() {};
    e.g = function() {
        return ""
    };
    e.qa = function() {
        this.u.qa()
    };

    function yc() {
        var a = window.location.hash.substring(1).split("/"),
            b = parseInt(a[0], 10);
        if (isNaN(b) || 0 > b) b = 0;
        a = 1 < a.length ? parseInt(a[1], 10) : 1;
        if (isNaN(a) || 1 > a) a = 1;
        return {
            wd: a,
            dc: b
        }
    }
    e.start = function() {
        var a = this;
        this.qa();
        this.A = window.k2pback.m2 ? !0 : !1;
        F(this.B, this.A ? [this.j[1] = h("input", {
            type: "radio",
            name: "pbpt",
            onchange: function() {
                zc(a, 1, 0)
            }
        }), "1", this.j[2] = h("input", {
            type: "radio",
            name: "pbpt",
            onchange: function() {
                zc(a, 2, 0)
            }
        }), "2"] : []);
        var b = yc();
        zc(this, b.wd, b.dc);
        F(this.I, window.k2pback.gdata ? h("a", {
            target: "_blank",
            href: window.k2pback.gdata
        }, "TXT") : [])
    };

    function zc(a, b, c) {
        a.f = b;
        var d = 1 < b ? b.toString() : "";
        a.j[b] && (a.j[b].checked = !0);
        a.v = "";
        "undefined" !== typeof window.k2pback.w && (a.v = window.k2pback["w" + d] + " - " + window.k2pback["b" + d]);
        F(a.J, h("b", [a.v]));
        b = a.Te(window.k2pback["m" + d]);
        a.Ya = 0;
        a.u.history(b.K, b.O);
        a.a = b.O;
        a.u.da(c || 0)
    }
    e.Ue = function(a) {
        var b = this,
            c = this.a.length;
        this.ia = a >= c ? c - 1 : a;
        this.Ya = (this.ia + 1) % 2;
        F(this.G, this.a.map(function(a, c) {
            return h("span", {
                style: this.ia == c ? {
                    color: "#c22"
                } : {},
                onclick: function() {
                    b.u.da(c)
                }
            }, [("undefined" === typeof this.fd.dg && 0 == c % 2 ? 1 + (c >> 1) + ". " : "") + a, this.fd.gg ? h("br") : " "])
        }, this));
        (a || window.location.hash) && window.history.replaceState && window.history.replaceState({}, "", "#" + a + (1 < this.f ? "/" + this.f : ""))
    };

    function Ac() {}
    Ac.prototype = new Bc;
    e = Ac.prototype;
    e.constructor = Ac;
    e.Ub = 0;
    e.kf = 16384;
    e.Pc = 1;
    e.Tb = 2;
    e.ae = 3;
    e.H = 0;
    e.ub = [];
    e.Na = [];
    e.la = 0;
    e.fa = 0;
    e.bb = 0;
    e.Ob = !1;
    e.zd = !1;
    e.wc = 0;
    e.Ka = 1;
    e.Ja = 1;
    e.Qe = 0;
    e.$d = function(a, b) {
        if (!(0 == b.length || 3 > a.length)) {
            for (var c = 2; c < a.length; c++) this.ub.push(a[c]);
            this.ub.push(this.Ub);
            return [b[0]]
        }
    };
    e.history = function(a) {
        this.ub = a
    };
    e.reset = function() {};
    e.Ae = function(a, b) {
        return String.fromCharCode(a + 97) + (this.fa - b)
    };
    e.da = function(a) {
        for (var b = a, c = 0; c < this.ub.length && 0 <= b;) this.ub[c] == this.Ub && b--, c++;
        this.Sa(null);
        this.reset();
        b = c;
        c = [];
        for (var d = -1, f, g, k, m, l, n = 0; n < b; n++) {
            var p = this.ub[n];
            if (0 < p) {
                if (f = p % this.H, g = Math.floor(p / this.H) % this.H, p = Math.floor(p / (this.H * this.H)), k = p % this.H, m = Math.floor(p / this.H) % this.H, p = Math.floor(p / (this.H * this.H)), l = this.u[g][f], null != l) {
                    l.color != d && (d = l.color, c.length = 0, c.push([f, g]));
                    c.push([k, m]);
                    if (f != k || g != m) null != this.u[m][k] && this.u[m][k].setPosition(-1, -1), l.setPosition(k,
                        m);
                    0 != p && p < this.wc && (l.type = p)
                }
            } else if (p != this.Ub)
                if (p = (-1 - p) % this.kf, f = p % this.H, g = Math.floor(p / this.H) % this.H, p = Math.floor(p / (this.H * this.H)), l = this.u[g][f], null != l) 0 == p % 2 ? l.setPosition(-1, -1) : 1 == p % 2 && (l.type = p >> 1);
                else
                    for (k = 0; k < this.M.length; k++)
                        if (null != this.M[k] && -1 == this.M[k].x && -1 == this.M[k].y) {
                            this.M[k].setParameters(p % 2, (p >> 1) % 16, f, g);
                            break
                        }
        }
        for (n = 0; n < c.length; n++) P(this, this.ae + n, !0, c[n][0], c[n][1]);
        for (n = c.length; n < this.Qe; n++) P(this, this.ae + n, !1, 0, 0);
        this.Qe = c.length;
        this.table.Ue(a);
        Q(this)
    };
    e.bf = function(a) {
        this.qb != a && (this.qb = a, this.zd && (this.Ob = 0 != (1 == this.bb ^ this.qb)), Q(this))
    };
    e.yd = function(a) {
        this.bb != a && (this.bb = a, this.Ob = this.zd ? 0 != (1 == this.bb ^ this.qb) : 1 == this.bb, Q(this))
    };

    function Cc(a, b, c) {
        if (null == a.D || !Dc(a, b, c)) return !1;
        b = (c * a.H + b) * a.H * a.H + (a.D.y * a.H + a.D.x);
        c = a.H * a.H * a.H * a.H;
        for (var d = a.Na.length - 1; 0 <= d; d--)
            if (0 <= a.Na[d] && Math.floor(a.Na[d] / c) < a.wc && a.Na[d] % c == b) return !0;
        return !1
    }

    function Ec(a, b, c) {
        if (!Dc(a, b, c)) return !1;
        b = c * a.H + b;
        c = a.H * a.H * a.H * a.H;
        for (var d = a.Na.length - 1; 0 <= d; d--)
            if (0 <= a.Na[d] && Math.floor(a.Na[d] / c) < a.wc && a.Na[d] % (a.H * a.H) == b) return !0;
        return !1
    }
    e.Ad = function(a) {
        return Ec(this, a.x, a.y)
    };

    function Fc(a, b) {
        return a.Ob ? a.la - 1 - b : b
    }

    function Gc(a, b) {
        return a.Ob ? a.fa - 1 - b : b
    }

    function Dc(a, b, c) {
        return 0 > b || b >= a.la || 0 > c || c >= a.fa ? !1 : !0
    }
    e.Qb = function(a) {
        u(a.Z, {
            left: Math.floor(this.Nb + Fc(this, a.x) * this.Ka) + "px",
            top: Math.floor(this.ib + Gc(this, a.y) * this.Ja) + "px"
        })
    };
    e.Qd = function(a) {
        a.Z.width = Math.round(this.Ka);
        a.Z.height = Math.round(this.Ja)
    };
    e.Sa = function(a, b, c, d, f) {
        Bc.prototype.Sa.call(this, a, b, c, d, f);
        a = this.D;
        P(this, this.Pc, null != a, a ? a.x : 0, a ? a.y : 0);
        P(this, this.Tb, !1, 0, 0)
    };
    e.setActive = function(a, b) {
        Bc.prototype.setActive.call(this, a, b);
        P(this, this.Pc, !1, 0, 0)
    };
    e.vc = function(a, b, c, d) {
        u(b, {
            display: "block",
            left: Math.round(this.Nb + Fc(this, c) * this.Ka) + "px",
            top: Math.round(this.ib + Gc(this, d) * this.Ja) + "px",
            width: Math.round(this.Ka - 1) + "px",
            height: Math.round(this.Ja - 1) + "px"
        })
    };

    function Hc(a, b, c) {
        return {
            x: Fc(a, Math.floor((b - a.Nb) / a.Ka)),
            y: Gc(a, Math.floor((c - a.ib) / a.Ja))
        }
    }

    function Ic(a, b, c) {
        var d = a.u[c][b];
        null != d && null != d.R && u(d.R.Z, {
            display: "none"
        });
        d = ((c * a.H + b) * a.H + a.D.y) * a.H + a.D.x;
        P(a, a.Tb, !0, b, c);
        var f = a.D.R;
        f.x = b;
        f.y = c;
        a.Qb(f);
        a = a.table;
        a.u.setActive(!1, void 0, "_move");
        b = [92, a.F, 1];
        "undefined" != typeof d && b.push(d);
        b.push(Math.floor((Date.now() - a.hb.j) / 100));
        a.send(b, null)
    }

    function Jc(a, b, c) {
        if (a.ve != b || a.we != c) a.Lb.innerHTML = Dc(a, b, c) ? a.Ae(b, c) : "", a.ve = b, a.we = c
    }
    e.td = function(a, b, c) {
        a = Hc(this, a, b);
        this.Lb && Jc(this, a.x, a.y);
        c || (null == this.D ? P(this, this.Pc, Ec(this, a.x, a.y), a.x, a.y) : P(this, this.Tb, Cc(this, a.x, a.y) || Ec(this, a.x, a.y), a.x, a.y))
    };
    e.Ce = function(a, b) {
        a = Hc(this, a, b);
        this.Lb && Jc(this, a.x, a.y);
        P(this, this.Tb, Cc(this, a.x, a.y), a.x, a.y)
    };
    e.De = function(a, b) {
        a = Hc(this, a, b);
        Cc(this, a.x, a.y) ? Ic(this, a.x, a.y) : this.Sa(null)
    };
    e.bc = function(a, b) {
        null != this.D && (a = Hc(this, a, b), Cc(this, a.x, a.y) && Ic(this, a.x, a.y))
    };

    function Bc() {}
    e = Bc.prototype;
    e.Ga = 1;
    e.table = null;
    e.b = null;
    e.Sb = null;
    e.oc = null;
    e.Wa = 0;
    e.Va = 0;
    e.Nb = 0;
    e.ib = 0;
    e.Rb = -1;
    e.Eb = !1;
    e.ka = !1;
    e.Wc = !1;
    e.Yb = !1;
    e.qb = !1;
    e.Zd = [];
    e.fc = 0;
    e.Se = !1;
    e.u = [];
    e.M = [];
    e.ua = [];
    e.D = null;
    e.ba = !1;
    e.Za = !1;
    e.Da = !1;
    e.We = !1;
    e.jd = 0;
    e.kd = 0;
    e.gd = 0;
    e.hd = 0;
    e.ld = 0;
    e.md = 0;
    e.ve = -1;
    e.we = -1;
    e.Lb = null;
    e.Fa = 0;
    e.xe = !1;
    e.W = !1;
    e.sb = !1;
    e.ma = [];
    e.Td = 0;
    e.ha = function(a, b) {
        this.table = a;
        this.Fa = b || 0;
        this.Ga = ta();
        this.f = this.table.app.S;
        this.Yb && !this.f && (this.Yb = !1);
        this.b = h(this.table.b, "div", {
            className: "bcont noth usno"
        });
        this.table.app.Vb ? this.xe = !0 : window.PointerEvent ? this.W = !0 : this.sb = !0;
        Kc(this);
        this.Sb = h(this.b, "canvas", {
            className: "noth",
            style: {
                position: "absolute",
                left: 0,
                top: 0,
                width: "100%",
                height: "100%",
                zIndex: 1
            }
        });
        this.oc = this.Sb.getContext("2d")
    };
    e.da = function() {};
    e.bf = function(a) {
        this.qb != a && (this.qb = a)
    };

    function Kc(a) {
        a.Yb || (a.b.onclick = function(b) {
            if (!a.ka) return !1;
            var c = b.target;
            if ("BUTTON" == c.tagName || "INPUT" == c.tagName || "IMG" == c.tagName && a.D && a.D.R && a.D.R.Z == c) return !0;
            c = R(a.b, b.clientX, b.clientY);
            a.bc(c.x, c.y, !1, 3 == b.which || b.altKey || b.ctrlKey);
            return !1
        });
        if (a.W || a.sb) a.W && u(a.b, {
            touchAction: "none"
        }), a.b.addEventListener(a.W ? "pointerdown" : "mousedown", function(b) {
            if (!a.ka || "BUTTON" == b.target.tagName) return !0;
            var c = "undefined" == typeof b.pointerId ? 0 : b.pointerId;
            if (a.Yb) return c = R(a.b, b.clientX,
                b.clientY), a.bc(c.x, c.y, !1, 3 == b.which || b.altKey || b.ctrlKey), b.stopPropagation(), !1;
            if (2 == a.Fa) {
                if (a.Da) return !0;
                a.a = c;
                Lc(a);
                R(a.b, b.clientX, b.clientY);
                b.stopPropagation();
                return !1
            }
            return 0 == a.Fa || "IMG" != b.target.tagName ? !0 : Mc(a, b.target, b.clientX, b.clientY) ? (a.W && (a.a = c, a.b.setPointerCapture(c)), b.stopPropagation(), b.preventDefault(), !1) : !0
        }, !1), a.b.addEventListener(a.W ? "pointermove" : "mousemove", function(b) {
            if (!a.ka && !a.Wc) return !0;
            var c = "undefined" == typeof b.pointerId ? 0 : b.pointerId;
            if (2 == a.Fa) {
                if (!a.Da ||
                    !a.W || a.a != c) return !0;
                R(a.b, b.clientX, b.clientY);
                return !1
            }
            if (a.ba) {
                if (!a.D || !a.W || a.a != c) return !0;
                Nc(a, b.clientX, b.clientY);
                return !1
            }
            if (a.W && "mouse" != b.pointerType) return !0;
            b = R(a.b, b.clientX, b.clientY);
            a.td(b.x, b.y, !a.ka || a.ba);
            return !1
        }, !1), a.b.addEventListener(a.W ? "pointerup" : "mouseup", function(b) {
            if (!a.ka) return !0;
            var c = "undefined" == typeof b.pointerId ? 0 : b.pointerId;
            if (2 == a.Fa) {
                if (!a.Da || !a.W || a.a != c) return !0;
                R(a.b, b.clientX, b.clientY);
                Oc(a);
                return !1
            }
            if (a.ba) {
                if (!a.D || !a.W || a.a != c) return !0;
                Pc(a,
                    b.clientX, b.clientY, 3 == b.which || b.altKey || b.ctrlKey);
                return !1
            }
            return !0
        }, !1), a.b.addEventListener(a.W ? "pointerout" : "mouseout", function(b) {
            if (!a.ka && !a.Wc) return !1;
            if (2 == a.Fa || a.ba || a.W && "mouse" != b.pointerType) return !0;
            b = R(a.b, b.clientX, b.clientY);
            a.td(b.x, b.y, !a.ka || a.ba);
            return !1
        }, !1);
        a.xe && Qc(a)
    }

    function Qc(a) {
        a.b.ontouchstart = function(b) {
            if (!a.ka || !b.touches || 1 != b.touches.length) return !0;
            b = b.touches[0];
            if ("BUTTON" == b.target.tagName) return !0;
            if (2 == a.Fa) {
                if (a.Da) return !0;
                a.Rb = b.identifier;
                Lc(a);
                R(a.b, b.clientX, b.clientY);
                return !1
            }
            return "IMG" != b.target.tagName ? !0 : Mc(a, b.target, b.clientX, b.clientY) ? (a.Rb = b.identifier, !1) : !0
        };
        a.b.ontouchmove = function(b) {
            if (!a.ka || !b.changedTouches) return !0;
            var c, d = null;
            for (c = b.changedTouches.length - 1; 0 <= c; c--)
                if (b.changedTouches[c].identifier == a.Rb) {
                    d = b.changedTouches[c];
                    break
                } if (!d) return !0;
            if (2 == a.Fa) {
                if (!a.Da) return !0;
                R(a.b, d.clientX, d.clientY);
                return !1
            }
            if (!a.ba || !a.D) return !0;
            Nc(a, d.clientX, d.clientY);
            return !1
        };
        a.b.ontouchend = function(b) {
            if (!a.ka || !b.changedTouches) return !0;
            var c, d = null;
            for (c = b.changedTouches.length - 1; 0 <= c; c--)
                if (b.changedTouches[c].identifier == a.Rb) {
                    d = b.changedTouches[c];
                    break
                } if (!d) return !0;
            if (2 == a.Fa) {
                if (!a.Da) return !0;
                R(a.b, d.clientX, d.clientY);
                Oc(a);
                return !0
            }
            if (!a.ba || !a.D) return !0;
            Pc(a, d.clientX, d.clientY, !1);
            a.Rb = -1;
            return !0
        }
    }
    e.Ad = function() {
        return !0
    };
    e.Sa = function(a, b, c, d, f) {
        b = "undefined" != typeof b ? b : !1;
        c = "undefined" != typeof c ? c : 0;
        d = "undefined" != typeof d ? d : 0;
        f = "undefined" != typeof f ? f : !1;
        this.D != a && this.D && (this.D.R || fa("ERR " + window.k2url.game + " PI/NULL/0 " + (a ? a.x + "," + a.y : "p=null") + ", s.x,y=" + this.D.x + "," + this.D.y + ", act:" + this.Eb), u(this.D.R.Z, {
            zIndex: 25
        }), this.Qb(this.D.R));
        (this.D = a) ? (u(this.D.R.Z, {
            zIndex: 50
        }), a.a = b, f && Rc(this, a.R.Z, c, d)) : this.ba && Sc(this)
    };

    function Mc(a, b, c, d) {
        for (var f = null, g = a.M.length - 1; 0 <= g; g--)
            if (a.M[g].R && b == a.M[g].R.Z) {
                f = a.M[g];
                break
            } if (!f) return !1;
        if (!a.D || a.D == f) {
            if (a.Ad(f)) return a.Sa(f, a.D == f, c, d, !0), !0
        } else if (a.Ad(f)) return a.Sa(f, !1, c, d, !0), !0;
        return !1
    }

    function Nc(a, b, c) {
        if (a.ba && a.D) {
            var d = a.D.R;
            d && (d.Z.style.left = b + a.jd + "px", d.Z.style.top = c + a.kd + "px");
            !a.Za && (5 < Math.abs(a.ld - b) || 5 < Math.abs(a.md - c)) && (a.Za = !0);
            a.Ce(b + a.gd, c + a.hd, a.Za)
        }
    }

    function Pc(a, b, c, d) {
        a.ba && a.D && (Sc(a), !a.Za && (5 < Math.abs(a.ld - b) || 5 < Math.abs(a.md - c)) && (a.Za = !0), a.Za ? a.De(b + a.gd, c + a.hd) : (a.D.a ? a.Sa(null) : a.Qb(a.D.R), b = R(a.b, b, c), a.bc(b.x, b.y, !0, d)))
    }

    function Rc(a, b, c, d) {
        a.ba = !0;
        b && (a.jd = parseInt(b.style.left, 10) - c, a.kd = parseInt(b.style.top, 10) - d, a.gd = a.jd + (b.width >> 1), a.hd = a.kd + (b.height >> 1), a.ld = c, a.md = d, a.Za = !1);
        a.sb && (a.b.ownerDocument.onmousemove = function(b) {
            Nc(a, b.clientX, b.clientY);
            return !1
        }, a.b.ownerDocument.onmouseup = function(b) {
            Pc(a, b.clientX, b.clientY, !1);
            return !1
        })
    }

    function Sc(a) {
        if (a.ba) {
            a.ba = !1;
            if (a.W && 0 <= a.a) {
                try {
                    a.b.releasePointerCapture(a.a)
                } catch (b) {}
                a.a = -1
            }
            a.sb && (a.b.ownerDocument.onmousemove = null, a.b.ownerDocument.onmouseup = null)
        }
    }

    function Lc(a) {
        a.Da = !0;
        a.W && 0 <= a.a && a.b.setPointerCapture(a.a);
        a.sb && (a.b.ownerDocument.onmousemove = function(b) {
            R(a.b, b.clientX, b.clientY);
            return !1
        }, a.b.ownerDocument.onmouseup = function(b) {
            R(a.b, b.clientX, b.clientY);
            Oc(a);
            return !1
        })
    }

    function Oc(a) {
        a.Da = !1;
        a.W && 0 <= a.a && (a.b.releasePointerCapture(a.a), a.a = -1);
        a.sb && (a.b.ownerDocument.onmousemove = null, a.b.ownerDocument.onmouseup = null)
    }

    function Tc(a, b, c, d) {
        "undefined" != typeof c ? (a.ma[b] = {
            wb: !1,
            rb: h(a.b, "div", {
                className: "bsbb",
                style: {
                    pointerEvents: "none",
                    position: "absolute",
                    display: "none",
                    zIndex: 5
                }
            })
        }, u(a.ma[b].rb, c)) : a.ma[b] = {
            wb: !1,
            rb: h(a.b, "div", {
                className: "bsbb",
                style: {
                    pointerEvents: "none",
                    position: "absolute",
                    display: "none",
                    zIndex: 5,
                    background: "#fff",
                    opacity: "0.5"
                }
            })
        };
        a.ma[b].Id = "undefined" != typeof d ? d : !1
    }

    function P(a, b, c, d, f) {
        a.ma[b] && (c ? (c = a.ma[b], c.wb = !0, c.Xe = d, c.Ye = f, c.Id || a.vc(b, c.rb, d, f)) : a.ma[b].wb && (a.ma[b].wb = !1, u(a.ma[b].rb, {
            display: "none"
        })))
    }
    e.vc = function() {};
    e.$d = function() {};
    e.history = function() {};
    e.Hd = function() {
        if (0 != this.ua.length) {
            for (var a = this.ua.length - 1; 0 <= a; a--) this.Qd(this.ua[a]);
            this.ma.forEach(function(a, c) {
                a.wb && !a.Id && this.vc(c, a.rb, a.Xe, a.Ye)
            }, this)
        }
    };
    e.qa = function() {
        var a = this.b.offsetWidth,
            b = this.b.offsetHeight,
            c = ta();
        if (a != this.Wa || b != this.Va || this.Ga != c) this.Ga = c, this.Wa = a, this.Va = b, this.Sb.width = Math.floor(a * this.Ga), this.Sb.height = Math.floor(b * this.Ga), this.oc.fillRect(0, 0, 1 * this.Ga, 1 * this.Ga), this.Hd(), Uc(this)
    };
    e.bc = function() {};
    e.td = function() {};
    e.Ce = function() {};
    e.De = function() {};
    e.Mf = function(a) {
        for (var b = this, c = arguments, d, f = 0, g = c.length; f < g; f++) this.Zd.push(d = h("img", {
            src: window.k2img[c[f]]
        })), d.complete || (this.fc++, d.onload = function() {
            b.fc--;
            0 == b.fc && b.Se && Uc(b)
        })
    };

    function Vc(a) {
        var b = 0 == a.fc;
        b || (a.Se = !0);
        return b
    }

    function Q(a) {
        !a.Td && 0 < a.Wa && 0 < a.Va && (a.Td = setTimeout(function() {
            a.Td = 0;
            Uc(a)
        }, 0))
    }

    function Uc(a) {
        if (Vc(a) && 0 != a.M.length) {
            var b, c = !!a.D && !!a.D.R;
            for (b = a.M.length - 1; 0 <= b; b--) a.M[b].R = null;
            for (b = a.ua.length - 1; 0 <= b; b--) {
                var d = a.ua[b];
                if (!(0 > d.y || d.y >= a.u.length || 0 > d.x || d.x >= a.u[d.y].length)) {
                    var f = a.u[d.y][d.x];
                    var g = null != f ? a.sd(f) : -1;
                    g != d.F ? d.x = d.y = -1 : f.R = d
                }
            }
            for (b = a.M.length - 1; 0 <= b; b--) f = a.M[b], 0 > f.x || 0 > f.y || null != f.R || (f.R = Wc(a, a.sd(f), f.x, f.y));
            c && !a.D.R && fa("ERR " + window.k2url.game + " PI/NULL/1 s.x,y=" + a.D.x + "," + a.D.y + ", act:" + a.Eb);
            f = a.D && a.ba ? a.D.R : null;
            for (b = a.ua.length - 1; 0 <=
            b; b--) d = a.ua[b], 0 <= d.x && 0 <= d.y ? d != f && Xc(a, d) : u(d.Z, {
                display: "none"
            });
            a.ma.forEach(function(a, b) {
                a.Id && a.wb && this.vc(b, a.rb, a.Xe, a.Ye)
            }, a)
        }
    }
    e.sd = function() {
        return -1
    };
    e.Qb = function() {};
    e.Qd = function() {};

    function Wc(a, b, c, d) {
        for (var f = 0, g = a.ua.length; f < g; f++) {
            var k = a.ua[f];
            if (0 > k.x && 0 > k.y && k.F == b) return k.x = c, k.y = d, k
        }
        b = new Yc(a.Zd[b].cloneNode(!0), b, c, d);
        a.b.appendChild(b.Z);
        a.ua.push(b);
        u(b.Z, {
            position: "absolute",
            zIndex: 25,
            display: "none"
        });
        a.Qd(b);
        return b
    }

    function Xc(a, b) {
        a.Qb(b);
        u(b.Z, {
            display: "block"
        })
    }
    e.yd = function() {};
    e.setActive = function(a, b) {
        this.Eb != a && (this.ka = this.Eb = a, !a && this.D && (this.Sa(null), "undefined" != typeof b && b && Q(this)), !a && this.Da && Oc(this), !a && this.We && (this.We = !1))
    };

    function Zc(a) {
        var b = a.oc,
            c = a.Ga,
            d = a.Wa * c;
        a = a.Va * c;
        b.fillStyle = "#e8b060";
        b.fillRect(0, 0, d, a)
    }

    function $c(a, b, c, d) {
        this.u = a;
        this.x = void 0 === d ? -1 : d;
        this.y = -1;
        this.color = void 0 === b ? 0 : b;
        this.type = void 0 === c ? 0 : c;
        this.R = null
    }
    $c.prototype.setParameters = function(a, b, c, d) {
        this.color = a;
        this.type = b;
        this.setPosition(c, d)
    };
    $c.prototype.setPosition = function(a, b) {
        -1 != this.x && -1 != this.y && (this.u[this.y][this.x] = null);
        this.x = a;
        this.y = b; - 1 != this.x && -1 != this.y && (this.u[this.y][this.x] = this)
    };

    function Yc(a, b, c, d) {
        this.Z = a;
        this.F = b;
        this.x = c;
        this.y = d
    }

    function ad(a, b, c) {
        var d = this;
        this.app = a;
        this.o = c;
        this.b = h(b, "div", c.cf);
        this.a = h(this.b, "div", c.re || {
            style: {
                position: "absolute",
                padding: "3px 4px 3px 8px",
                top: 0,
                left: 0,
                right: 0,
                bottom: "2.5em",
                overflowY: "scroll",
                WebkitOverflowScrolling: "touch"
            }
        });
        u(this.a, {
            wordWrap: "break-word",
            background: "#fff"
        });
        this.a.onselectstart = function(a) {
            a.stopPropagation();
            return !0
        };
        a = h(this.b, "form", c.sf || {});
        b = h(a, "div", {
            style: {
                display: "table",
                width: "100%"
            }
        });
        c.re || u(b, {
            position: "absolute",
            bottom: 0
        });
        b = h(b, "div", {
            style: {
                display: "table-cell"
            }
        });
        this.gb = h(b, "input", c.cg || {
            className: "bsbb",
            style: {
                width: "100%",
                margin: 0
            }
        });
        Object.assign(this.gb, {
            name: "somename",
            type: "text",
            autocomplete: "off",
            autocapitalize: "off"
        });
        a.onsubmit = function() {
            var a = d.gb.value.trim();
            d.gb.value = "";
            0 < a.length && d.o.ue(a);
            return !1
        }
    }
    var N = "+ ";
    ad.prototype.reset = function() {
        this.a.innerHTML = "";
        this.gb.value = ""
    };
    ad.prototype.Ea = function() {
        if (this.o.Jd && !this.app.ra) 0 < this.b.clientHeight && ((document.scrollingElement || document.documentElement).scrollTop = document.documentElement.scrollHeight - document.documentElement.clientHeight);
        else {
            var a = this.a,
                b = a.scrollHeight - a.clientHeight;
            0 < b && (a.scrollTop = b)
        }
    };
    ad.prototype.append = function(a, b) {
        var c = this,
            d;
        a = a.split("\n");
        var f = this.a,
            g = this.o.Jd && !this.app.ra ? document.body : this.a,
            k = g.scrollTop + 2 >= g.scrollHeight - g.clientHeight;
        b && a.reverse();
        for (var m = 0; m < a.length; m++) {
            var l = a[m],
                n = b ? f.firstChild : null;
            if (0 == l.indexOf(N)) {
                var p = l.substring(N.length);
                2 < p.length && "[" == p[0] && "]" == p[p.length - 1] && (l = parseInt(p.substring(1, p.length - 1), 10), isNaN(l) || (p = function(a) {
                    a = parseInt(a, 10);
                    return 10 > a ? "0" + a : a
                }, l = new Date(1E3 * l), p = l.toLocaleDateString() == (new Date).toLocaleDateString() ?
                    p(l.getHours()) + ":" + p(l.getMinutes()) : l.getFullYear() + "-" + p(l.getMonth() + 1) + "-" + p(l.getDate())));
                nc(bd(f, h("div", {
                    className: "tind"
                }, ["+ " + p]), n))
            } else 1 < l.length && "[" == l[0] && "]" == l[l.length - 1] ? bd(f, h("div", {
                className: "mtbq"
            }, h("button", {
                onclick: function(a) {
                    this.parentNode.parentNode.removeChild(this.parentNode);
                    c.o.$e && c.o.$e();
                    a.stopPropagation();
                    return !1
                }
            }, [l.substring(1, l.length - 1)])), n) : -1 != (d = l.indexOf(":")) ? nc(bd(f, h("div", {
                className: "tind"
            }, [h("b", [l.substring(0, d)]), l.substring(d)]), n)) : h(f,
                "div", [l])
        }
        this.o.Jd && !this.app.ra ? !b && 0 < this.b.clientHeight && document.documentElement.scrollHeight > document.documentElement.clientHeight && ((document.scrollingElement || document.documentElement).scrollTop = document.documentElement.scrollHeight) : k && (g.scrollTop = g.scrollHeight - g.clientHeight + 1)
    };
    var cd = '<span class="emo">$&</span>',
        ha = 99,
        va = 101,
        Ea = 512,
        Ba = 17,
        Da = 18,
        Ca = 19,
        yb = 20,
        rb = 21,
        gc = 22,
        za = 23,
        Nb = 24,
        Ob = 25,
        Sb = 27,
        Wb = 28,
        Tb = 29,
        bc = 30,
        ya = 31,
        dc = 32,
        hc = 33,
        wb = 34,
        pb = 35,
        Fa = 20,
        dd = 22,
        Aa = 51,
        Kb = 52,
        $b = 53,
        Ha = 52,
        ic = 61,
        Wa = 61,
        ka = 63,
        la = 64,
        Eb = 70,
        Db = 71,
        Ib = 72,
        Ab = 73,
        Cb = 74,
        Lb = 75,
        ac = 76,
        ec = 77,
        Qa = 71,
        bb = 72,
        Mb = 74,
        fc = 76,
        Ua = 77,
        nb = 80,
        ub = -2,
        tb = 0,
        sb = 1,
        ed = 1,
        fd = 2,
        S;

    function gd(a, b, c) {
        this.tab = a;
        this.o = c;
        this.o.Xa = this.o.Xa || 0;
        this.j = 0;
        this.f = -1;
        this.v = this.o.mc + (this.o.ed ? 1 : 0);
        var d = this;
        this.b = h(b, "div", {
            style: {
                position: "absolute",
                width: "100%",
                height: "100%"
            }
        });
        this.va = h(this.b, "div", {
            style: {
                position: "absolute",
                left: 0,
                right: 0,
                top: 0,
                bottom: 0,
                overflowY: "scroll",
                WebkitOverflowScrolling: "touch",
                background: "#fff"
            }
        });
        this.a = h(this.va, "table", {
            className: "br",
            style: {
                width: "100%",
                borderCollapse: "collapse"
            }
        });
        if (this.o.ab || this.o.vd) q(this.va, "mb1s"), this.o.ab && (this.a.onclick =
            function(a) {
                a = a.target;
                "TD" == a.tagName && a.cellIndex >= d.o.Xa && (a = a.parentNode.rowIndex * d.v + a.cellIndex - d.o.Xa, a != d.f && a < d.j && hd(d.tab, a));
                return !1
            }), a = h(this.b, "div", {
            className: "lh1s",
            style: {
                position: "absolute",
                left: 0,
                right: 0,
                bottom: 0
            }
        }), this.o.vd && h(a, [h("button", {
            className: "minw",
            onclick: function() {
                d.tab.send([94, d.tab.F], null)
            }
        }, this.o.vd), " "]), this.o.ab && h(a, [h("button", {
            className: "minw",
            onclick: function() {
                0 <= d.f - 1 && hd(d.tab, d.f - 1)
            }
        }, "<"), " ", h("button", {
            className: "minw",
            onclick: function() {
                d.f +
                1 < d.j && hd(d.tab, d.f + 1)
            }
        }, ">")]);
        a = this.o.Xa || this.o.ed;
        this.A = "10%";
        this.G = this.B = (a ? Math.round(90 / this.o.mc) : Math.round(100 / this.o.mc)) + "%";
        a && 2 == this.o.mc && (this.B = "40%")
    }
    gd.prototype.reset = function() {
        this.j = 0;
        this.f = -1;
        for (var a = this.a.rows.length - 1; 0 <= a; a--) this.a.deleteRow(a)
    };

    function id(a) {
        return 11 < a.length ? a.substring(0, 9) + ".." : a
    }

    function jd(a, b) {
        for (var c = a.j, d, f, g = 0, k = 0; k < b.length; k++)
            if ("=" == b[k] || "_" == b[k]) k += a.v - 1;
            else if (0 == a.j % a.v && (d = a.a.insertRow(-1), f = k + a.v < b.length ? b[k + 2] : "", g = "=" == f ? 2 : "_" == f ? 1 : 0, a.o.Xa && d && (F(f = d.insertCell(-1), [1 + Math.floor(a.j / a.v) + ""]), 1 == a.a.rows.length && (f.width = a.A))), a.j++, d = a.a.rows[a.a.rows.length - 1]) F(f = d.insertCell(-1), [id(b[k])]), a.o.ab && u(f, {
                cursor: "pointer"
            }), 1 == a.a.rows.length && (f.width = a.o.ed && 0 == (a.j - 1) % a.v ? a.A : 2 == d.cells.length ? a.B : a.G), g && (f.style.borderBottom = 1 == g ? "dashed 1px #000" :
                "solid 2px #000");
        a.o.ab ? a.Ea(a.j - c) : a.va.scrollTop = a.va.scrollHeight - a.va.clientHeight
    }

    function kd(a, b, c) {
        var d = a.a.rows[Math.floor(b / a.v)];
        if (d) {
            var f;
            (f = d.cells[b % 2 + a.o.Xa]) && q(f, "fb", c)
        }
    }

    function ld(a, b) {
        a.o.ab && (-1 != a.f && kd(a, a.f, !1), a.f = b, -1 != b && kd(a, a.f, !0))
    }
    gd.prototype.Ea = function(a) {
        -1 == this.tab.zb && this.f < this.j - 1 - a || (ld(this, this.j - 1), this.va.scrollTop = this.va.scrollHeight - this.va.clientHeight)
    };

    function md(a) {
        return a.f == a.j - 1
    }
    var nd = 0,
        od = 0;

    function oc(a, b) {
        var c = this;
        this.app = a;
        this.Oa = this.app.g("gname");
        this.P = b;
        this.a = {};
        this.f = [];
        this.A = 1;
        this.L = !1;
        var d = this;
        this.b = document.getElementById("pretl") || h(this.app.A, "div", {
            style: {
                display: "none"
            }
        });
        u(this.b, {
            display: "none"
        });
        q(this.b, "tblobby");
        q(this.b, "usno");
        this.V = h(this.b, "div", {
            className: "tbvusers"
        });
        this.v = document.getElementById("pretabs") || h(this.b, "div");
        q(this.v, "tbvtabs");
        var f, g, k;
        bd(this.b, a = h("div", {
            className: "newtab2 dcpd"
        }), this.b.firstChild);
        h(a, [f = h("button", {
            className: "minwd",
            onclick: function() {
                window.location.href = "uniwebview://out3?data=" + JSON.stringify([Qa]);
                d.app.send([Qa], null)
            }
        }, this.app.g("bl_newtab")), " ", h("div", {
            className: "selcwr mro"
        }, [g = h("button", {
            className: "selcbt min85"
        }, h("div", "-")), k = h("select", {
            className: "selcsl",
            onchange: function(a) {
                (a = a.target.options[a.target.selectedIndex]) && d.app.Y("/join " + a.text.split(" ")[0])
            }
        })]), h("div", {
            className: "ib"
        }, [h("button", {
            className: "ubut",
            onclick: function() {
                L(d.app, d.app.V + (d.L ? "" : "/p"));
                return !1
            }
        }, h("div", {
            className: "uicon"
        }))])]);
        w(this.app, "rooms", function(a) {
            k.options.length =
                0;
            h(k, a.list.map(function(b, c) {
                return h("option", c == a.Ab ? {
                    selected: !0
                } : {}, b)
            }));
            F(g, [a.list[0 <= a.Ab ? a.Ab : 0].split(" ")[0] || "-"])
        });
        w(this.app, "tumode", function(a) {
            f.disabled = !!a
        });
        h(this.v, "div", {
            className: "tldeco"
        });
        this.N = h(this.v, "div");
        this.G = h(this.v, "div", {
            className: "chpan dcpd"
        });
        h(this.G, "div", {
            className: "chtop"
        }, [this.J = h("select", {
            className: "chgrlist minwd",
            onchange: function(a) {
                var b = a.target.selectedIndex;
                d.app.Y("/g " + (0 == b ? "-" : 1 == b ? "+" : a.target.value.split(" ")[0]))
            }
        }), h("input", {
            type: "checkbox",
            onchange: function() {
                var a = this.checked;
                q(d.G, "chopen", a);
                a && d.j.Ea()
            }
        }), this.app.g("bl_whisper"), " ", this.ea = h("span")]);
        a = h(this.G, "div", {
            className: "chsub"
        });
        this.j = new ad(this.app, a, {
            ue: function(a) {
                d.app.Y(a)
            },
            cf: {
                style: {
                    display: "block",
                    position: "relative",
                    width: "100%",
                    maxWidth: "640px",
                    height: "140px"
                }
            }
        });
        u(this.j.a, {
            border: "solid 1px #ccc"
        });
        w(this.app, "chmode", function(a) {
            return q(c.b, "chmode", !!a)
        });
        this.B = h(this.v, "div", {
            className: "tlst"
        });
        w(this.app, "tumode", function(a) {
            a = a ? 0 : 1;
            a != d.A && (d.A =
                a)
        });
        var m = h(this.V, "div", {
            className: "ulpan",
            style: {
                display: "none"
            }
        }, [h("button", {
            onclick: function() {
                var a = d.I;
                pd(a, a.Ua ? 0 : 1);
                return !1
            }
        }, this.app.g("tu_bcan"))]);
        w(this.app, "urole", function(a) {
            u(m, {
                display: a == ed ? "block" : "none"
            })
        });
        var l = this.app.o.vb;
        this.I = new qd(this.app, this.V, {
            Ke: !0,
            cols: l ? [T, -1] : [U, T],
            Rd: !1,
            dd: function(a, b) {
                var c = a.name,
                    f = d.app.uc;
                1 == b.Ua && f == ed || od ? (pd(b, 0), Ta(d.app, c)) : f == fd || nd ? Va(d.app, c, a.X) : d.app.send([Wa], [c])
            },
            pf: !0
        });
        db(this.app, this.I);
        w(this.app, "tumode", function(a) {
            var b =
                    d.I,
                c = 0 != a ? [rd, U] : l ? [T, -1] : [U, T];
            a = 0 != a ? rd : 0;
            a = void 0 === a ? 0 : a;
            b.cols = c;
            b.A && (b.A = sd(b, b.A));
            td(b, a)
        })
    }
    oc.prototype.onshow = function() {
        this.j && this.j.Ea();
        this.$ || (this.$ = !0, "function" === typeof window.k2adload && setTimeout(function() {
            window.k2adload()
        }, 0))
    };
    oc.prototype.Pb = function() {
        var a = "p" == Na();
        this.L != a && (this.L = a, q(this.b, "tbact", a))
    };

    function zb(a, b) {
        var c = b.split(" ");
        "/c" == c[0] ? a.j.a.innerHTML = "" : "/g" == c[0] ? (q(a.G, "chgrp"), b = parseInt(c[1], 10), c = c.slice(2), F(a.ea, [0 < c.length ? "(" + c.length + ")" : ""]), c.unshift("-", "START"), a.J.options.length = 0, h(a.J, c.map(function(a) {
            return h("option", {}, a)
        })), 0 <= b && (a.J.selectedIndex = b + 2)) : a.j.append(b)
    }

    function ud(a, b, c) {
        var d, f = 0;
        if (1 == a.P) var g = h("div", {
            className: "tplone tavail"
        }, [h("div", {
            className: "tpllist"
        }, b.O[vd + 0] || "-"), h("div", {
            className: "tpar0"
        }, b.O[0])]);
        else {
            g = h("div", {
                className: "tplbl"
            });
            for (d = b.Ld - 1; 0 <= d; d--) {
                var k = 0 == b.K[wd + d] && xd(b) && b.app.Ha >= yd(b),
                    m = b.O[vd + d],
                    l = cb(a.app, m);
                bd(g, h("div", {
                    className: k ? "tplemp" : "" != m ? "tplnorm" : "tplunav"
                }, [h("div", {
                    className: l ? "r" + La(a.app, l.f) : "rnone"
                }), m || "\u2013", h("span", {
                    className: "tplrn snum"
                }, l ? 0 != (l.a & lc) ? "X" : J(a.app, l.f) : "-")]), k || "" == m ? null :
                    g.firstChild);
                k && f++
            }
            h(g, "div", {
                className: "tpar0"
            }, [h("div", {
                className: "rnone"
            }), b.O[0]])
        }
        return zd(a.B, h("a", {
            className: "awrap dcpd" + (0 < f ? " tavail" : ""),
            onclick: function() {
                ab(a.app, b.F);
                return !1
            }
        }, h("div", {
            className: "tmaxw"
        }, [h("div", {
            className: "tnum"
        }, 2 == b.K[2] ? "-" : "#" + b.F), h("div", {
            className: "tpar1"
        }, b.O[0]), g, 1 < a.P ? h("div", {
            className: "tjoin"
        }, h("button", {
            className: "butbl"
        }, ">>")) : null])), c)
    }

    function Gb(a, b, c) {
        a.a.hasOwnProperty(b[1]) || (b = new Ad(a.app, b, c), a.a[b.F] = {
            $a: b
        }, b.C = ud(a, b), Bd(a, b))
    }

    function Jb(a, b) {
        if (a.a.hasOwnProperty(b)) {
            var c = a.a[b].$a;
            c.C && (a.B.removeChild(c.C), c.C = null);
            c = a.f.indexOf(c); - 1 != c && a.f.splice(c, 1);
            delete a.a[b]
        }
    }

    function Fb(a, b, c) {
        if (a.a.hasOwnProperty(b[1])) {
            var d = a.a[b[1]].$a;
            Dd(d, b, c);
            b = a.f.indexOf(d); - 1 != b && (d.C = ud(a, d, d.C), a.f.splice(b, 1), Bd(a, d))
        }
    }

    function Sa(a, b, c) {
        cc(a);
        a.a = {};
        a.f.length = 0;
        for (var d = b[1], f = 3, g = 0, k = d, m = b[2]; !(f >= b.length);) {
            -1 == d && (k = b[f], f++, m = vd + (k - (wd - 1)));
            if (f + k > b.length || g + m > c.length) break;
            var l = new Ad(a.app, b.slice(f - 1, f - 1 + k + 1), c.slice(g, g + m));
            a.a[l.F] = {
                $a: l
            };
            l.C = ud(a, l);
            Bd(a, l);
            g += m;
            f += k
        }
    }

    function Bd(a, b) {
        for (var c = a.f.length, d = a.app.Ha; 0 < c;) {
            var f = a.f[c - 1];
            var g = f.K[1],
                k = b.K[1];
            g = g < k ? -1 : g > k ? 1 : 0;
            if (0 != a.A) {
                k = Ed(f);
                var m = Ed(b),
                    l = Fd(f),
                    n = Fd(b);
                g = -g;
                f = 0 < m && 0 == n ? 0 < k && 0 == l ? g : 1 : 0 < k && 0 == l ? -1 : Gd(b, d) && 0 < m ? Gd(f, d) && 0 < k ? g : -1 : Gd(f, d) && 0 < k ? 1 : 2 == b.K[2] ? 2 == f.K[2] ? g : 1 : 2 == f.K[2] ? -1 : g
            } else f = g;
            0 == a.A && (f = -f);
            if (0 <= f) break;
            c--
        }
        d = a.f[c];
        a.f.splice(c, 0, b);
        b.C && a.B.insertBefore(b.C, d ? d.C : null)
    }

    function cc(a) {
        Object.keys(a.a).forEach(function(a) {
            var b = this.a[a].$a;
            b.C && (this.B.removeChild(b.C), b.C = null);
            delete this.a[a]
        }, a)
    }

    function Hd() {}
    e = Hd.prototype;
    e.ca = 0;
    e.Oe = 0;
    e.xc = 0;
    e.kc = !1;
    e.xd = !1;
    e.name = null;
    e.Yc = null;
    e.Kb = null;
    e.ha = function(a, b, c, d) {
        b = !!b;
        this.Oe = a;
        this.kc = b;
        this.ca = a + a % 2;
        this.xc = 0;
        this.Kb = d || null;
        this.name = Array(this.ca);
        this.Yc = Array(this.ca);
        this.a = Array(this.ca);
        this.focus = Array(this.ca);
        b && (this.X = Array(this.ca));
        c && (this.time = Array(this.ca));
        for (a = 0; a < this.ca; a++) this.name[a] = "", this.a[a] = 0, this.Yc[a] = "", this.time && (this.time[a] = "0:00"), this.X && (this.X[a] = "-"), this.focus[a] = !0
    };
    e.reset = function() {
        for (var a = 0; a < this.ca; a++) {
            var b = a;
            this.a[b] = 1;
            this.name[b] = "";
            this.X && (this.X[a] = "-");
            Id(this, a, 0);
            this.Yc[a] = "";
            this.focus[a] = !0
        }
        this.xc = 0
    };

    function Id(a, b, c, d) {
        a.time && (a.time[b] = 0 > c ? "(?)" : "" + Math.floor(c / 60) + ":" + Math.floor(c % 60 / 10) + c % 60 % 10 + ("undefined" != typeof d && 0 < d ? "/" + d : ""))
    }

    function Jd() {}
    e = Jd.prototype;
    e.F = 0;
    e.U = 0;
    e.oa = 0;
    e.zb = -1;
    e.Ya = -1;
    e.hf = 0;
    e.Ec = null;
    e.ef = !1;
    e.$b = !1;
    e.qd = 0;
    e.app = null;

    function Kd(a) {
        return 0 != (a.U & 4) && (0 == (a.U & 2) || 0 != (a.U & 2) && !V(a)) && (xd(a.pa) && a.app.Ha >= yd(a.pa) || 0 != (a.oa & 2)) && !W(a)
    }

    function V(a) {
        return 0 != (a.U & 1)
    }

    function W(a) {
        return 0 != (a.oa & 4)
    }
    e.g = function(a) {
        return this.app.g(a)
    };
    e.send = function(a, b) {
        this.app.send(a, b)
    };

    function Ld(a, b, c) {
        a.send([82, a.F, c], [b])
    }
    e.Y = function(a) {
        a.length && (a.length > Ea && (a = a.substring(0, Ea) + "..."), this.send([81, this.F], [a]))
    };

    function Md() {}
    Md.prototype = new Jd;
    e = Md.prototype;
    e.constructor = Md;
    e.Ed = !1;
    e.b = null;
    e.u = null;
    e.pa = null;
    e.T = null;
    e.wa = null;
    e.history = null;
    e.lb = null;
    e.hb = null;
    e.xb = [];
    e.bd = null;
    e.$c = null;
    e.cd = null;
    e.ad = null;
    e.Zc = null;
    e.na = null;
    e.eb = 0;
    e.Bb = null;
    e.qc = !0;
    e.qe = !1;
    e.Vc = !1;
    e.Pe = !1;
    e.ha = function(a, b, c) {
        this.b || (this.app = a, this.Rf = this.Ud = this.Sf = this.Vd = !0, this.a = c, this.a.Ve = this.a.Ve || !1, this.a.Kd = this.a.Kd || null, this.a.cc = this.a.cc || null, this.a.Md = this.a.Md || !1, this.a.Ge = this.a.Ge || !1, this.a.Ie = this.a.Ie || !1, this.a.He = this.a.He || !1, this.a.Le = this.a.Le || 2, this.a.Fe = this.a.Fe || !1, this.T = new Hd, this.T.ha(this.app.Jb, this.a.Ve, !0, this.a.Kd), this.b = h(this.app.A, "div", {
            className: "gview",
            style: {
                display: "none"
            }
        }), this.app.mb || u(this.b, {
            minHeight: "460px"
        }), Nd(this), Od(this), this.u =
            new b, this.u.ha(this), u(this.b, {
            background: this.u.jf
        }), Pd(this), new Qd(this.u.b, {
            className: "ctcont cttac ctst0",
            style: {
                zIndex: 70
            }
        }), this.hb = new Rd(this), Sd(this), a = this.app.ta, Td(a && this.app.If ? "click" : a && this.app.G ? "touchstart" : null))
    };

    function Sd(a) {
        a.app.S && a.a.Fe && (window.onkeypress = function() {
            return !0
        })
    }
    e.Ue = function() {
        q(this.b, "tstatinbohide", !md(this.history))
    };

    function Nd(a) {
        h(a.b, "div", {
            className: "thnavcont usno tama"
        }, [h("button", {
            className: "xbut hdhei",
            style: {
                zIndex: ha + 1,
                right: 0,
                width: "44px"
            },
            onclick: function() {
                Ud(a)
            }
        }, "X"), h("button", {
            className: "cmenubut hdhei",
            style: {
                zIndex: ha + 1,
                right: "38px",
                width: "44px"
            },
            onclick: function() {
                this.blur();
                Vd(a, !M(a.b, "sbdropvis"))
            }
        }, h("div", {
            className: "cmenu"
        }))]);
        var b = h(a.b, "div", {
            className: "thead bsbb usno hdhei",
            style: {
                zIndex: ha
            }
        });
        b = h(b, "div", {
            style: {
                display: "table",
                width: "100%",
                height: "100%"
            }
        });
        b = h(b, "div", {
            style: {
                display: "table-cell",
                verticalAlign: "middle",
                textAlign: "center"
            }
        });
        Wd(a, b)
    }

    function Wd(a, b) {
        w(a.app, "tabplayers", function() {
            if (0 >= a.F) F(b, []);
            else {
                for (var c = a.T, d = 2 == c.Oe ? 2 : a.T.ca, f = h("div", {
                    style: 2 < d ? {
                        display: "table",
                        margin: "0 auto",
                        fontSize: "13px",
                        lineHeight: "1.2",
                        paddingRight: "8px"
                    } : {
                        display: "table",
                        margin: "0 auto",
                        paddingRight: "8px"
                    }
                }), g, k = 3 < d ? 2 : 1, m = 3 == d ? 3 : 2, l = 0; l < m; l++) {
                    var n = h(f, "div", {
                            style: {
                                display: "table-cell",
                                textAlign: l < m - 1 ? "right" : "left",
                                width: 2 == m ? "50%" : "auto",
                                padding: "0 .4em",
                                whiteSpace: "nowrap"
                            }
                        }),
                        p = h(n, "div", {
                            className: "ib",
                            style: {
                                textAlign: "left",
                                marginRight: ".4em"
                            }
                        });
                    for (g = 0; g < k; g++) 0 < g && h(p, "br"), 2 < d && c.Kb && h(p, "div", {
                        className: "ib",
                        style: {
                            width: "3px",
                            height: ".8em",
                            background: c.Kb[2 * g + l],
                            marginRight: "3px"
                        }
                    }), h(p, "b", {}, [c.X && c.kc ? c.X[2 * g + l] : "#" + (2 * g + l + 1)]);
                    p = h(n, "div", {
                        className: "ib",
                        style: {
                            textAlign: "right"
                        }
                    });
                    for (g = 0; g < k; g++) 0 < g && h(p, "br"), c.time && h(p, [c.time[2 * g + l] + ""])
                }
                F(b, f)
            }
        })
    }

    function Od(a) {
        var b;
        a.ya = h(a.b, "div", {
            className: "bsbb tsb sbclrd"
        });
        var c = h(a.ya, "div", {
            className: "tsbinner bsbb"
        });
        h(c, "div", {
            className: "ttlcont"
        }, [h("div", {
            className: "ttlnav"
        }, [b = h("button", {
            className: "butsys butlh",
            onclick: function() {
                $a(a.app)
            }
        }, "\u2013"), h("button", {
            className: "butsys butlh",
            onclick: function() {
                Ud(a)
            }
        }, "X")]), a.$ = h("div", ["-"])]);
        w(a.app, "chatalert", function(a) {
            return q(b, "alert", a)
        });
        w(a.app, "tabopen", function() {
            return F(a.$, [Xd(a)])
        });
        w(a.app, "tabstatus", function() {
            return F(a.$,
                [Xd(a)])
        });
        Yd(a, c);
        var d = h(c, "div", {
            className: "tsinsb lh1s",
            style: {
                textAlign: "center",
                background: "#f21",
                color: "#fff",
                paddingLeft: ".5em",
                paddingRight: ".5em"
            }
        });
        a.ta = h(d, "div", {
            className: "tstatlabl nowrel"
        }, "-");
        h(d, "div", {
            className: "tstatstrl"
        }, [h("button", {
            className: "butwb",
            style: {
                minWidth: "8em"
            },
            onclick: function() {
                a.send([85, a.F], null);
                return !1
            }
        }, a.g("bl_start"))]);
        Zd(a, c);
        a.f.Lf()
    }

    function Yd(a, b) {
        var c = h(b, "div");
        $d(a, c);
        w(a.app, "tabplayers", function() {
            return $d(a, c)
        })
    }

    function Zd(a, b) {
        ae(a, b);
        a.f = be(a, b);
        b = a.f;
        ce(a, b.yb);
        b.add(a.g("sw_chat"), a.wa.b);
        b = a.f;
        a.a.Ge || (a.history = new gd(a, b.yb, {
            mc: a.a.Le,
            ab: !a.a.Ie,
            vd: a.a.cc,
            ed: a.a.$f ? !0 : !1,
            Xa: a.a.He ? 0 : 1
        }), a.ea = b.add(a.app.S ? a.g("sw_history") : null, a.history.b));
        de(a, a.f);
        ee(a, a.f)
    }

    function be(a, b) {
        a = {
            nd: {},
            Pa: {},
            Kf: a.app.G,
            ha: function(a) {
                this.b = h(a, "div", {
                    className: "tcrdcont"
                }, [h("div", {
                    className: "tcrdtabcont"
                }, this.va = h("div", {
                    className: "tcrdtab"
                })), this.yb = h("div", {
                    className: "tcrdpan"
                })])
            },
            add: function(a, b) {
                var c = this,
                    d = Object.keys(this.Pa).length;
                this.Pa[d] = b;
                u(b, {
                    visibility: d ? "hidden" : "inherit"
                });
                a && h(this.va, "div", {
                    className: "tcrdcell"
                }, this.nd[d] = h("button", {
                    className: d ? "" : "active",
                    onclick: function() {
                        return c.show(d)
                    }
                }, [a]));
                return d
            },
            show: function(a) {
                Object.keys(this.nd).forEach(function(b) {
                    q(this.nd[b],
                        "active", a == b)
                }, this);
                Object.keys(this.Pa).forEach(function(b) {
                    u(this.Pa[b], {
                        visibility: a == b ? "inherit" : "hidden"
                    })
                }, this);
                if (this.Kf) {
                    var b = this.Pa[a].firstChild.scrollTop;
                    this.yb.insertBefore(this.Pa[a], null);
                    this.Pa[a].firstChild.scrollTop = b
                }
            },
            Lf: function() {
                this.yb.insertBefore(this.Pa[0], null)
            }
        };
        a.ha(b);
        return a
    }

    function fe(a) {
        a.I || (a.I = z(a.app, a.g("bl_invite"), {
            width: "81%",
            minWidth: "280px",
            maxWidth: "320px"
        }, {
            nopad: !0
        }), a.ja = new qd(a.app, a.I.Ca, {
            Ke: !0,
            cols: [U, T],
            Pf: T,
            Rd: !0,
            dd: function(b) {
                v(a.app);
                a.send([95, a.F, 0], [b.name])
            },
            df: {
                className: "ovysct",
                style: {
                    width: "100%",
                    height: "300px",
                    borderTop: "solid 1px #ddd"
                }
            }
        }), db(a.app, a.ja));
        E(a.app, a.I)
    }
    e.Ba = null;

    function Pd(a) {
        a.Aa = h(a.u.b, "div", {
            className: "tsinbo bsbb",
            style: {
                position: "absolute",
                width: "100%",
                height: "100%",
                textAlign: "center",
                zIndex: 70
            }
        });
        var b = h(a.Aa, "div", {
            style: {
                display: "table-cell",
                verticalAlign: "middle"
            }
        });
        b = h(b, "div", {
            className: "bsbb bs ib",
            style: {
                textAlign: "center",
                maxWidth: "80%",
                minWidth: "35%",
                padding: "0.75em 2em",
                border: "solid 3px #fff",
                background: "#e21",
                color: "#fff"
            }
        });
        a.ra = h(b, "div", {
            className: "tstatlabl fb"
        }, "");
        b = h(b, "div", {
            className: "tstatstrl"
        });
        h(b, "button", {
            className: "butwb",
            style: {
                marginTop: ".25em",
                minWidth: "8em"
            },
            onclick: function() {
                a.send([85, a.F], null);
                return !1
            }
        }, a.g("bl_start"))
    }

    function he(a, b, c) {
        if (a.Ba != b || a.za != c) {
            var d = a.Ba,
                f = a.Ec;
            a.Ba = b;
            a.za = !!c;
            a.ra.innerHTML = a.Ba ? (f ? f + "<br />" : "") + a.Ba : "-";
            a.ta.innerHTML = a.Ba || "-";
            q(a.b, "tstatstart", null != b && !!c);
            q(a.b, "tstatact", null != a.Ba);
            !!d == (null != a.Ba) || Vd(a, null != a.Ba)
        }
    }

    function ie(a, b, c) {
        if (a.na) {
            var d = a.eb;
            a.eb = b;
            F(a.sa, [(c ? je(a, b, c[c.length - 1]) : "") || "-"]);
            a.na.show(a.eb ? 1 : 0);
            d != a.eb && Vd(a, 0 != a.eb)
        }
    }

    function X(a, b, c) {
        b = [93, a.F, b];
        "undefined" != typeof c && b.push(c);
        b.push(Math.floor((Date.now() - a.hb.j) / 100));
        a.send(b, null)
    }

    function ke(a) {
        var b = [];
        a.a.Ee && b.push(a.$c = h("button", {
            className: "minw",
            onclick: function() {
                X(a, 1, 1)
            }
        }, a.g("bl_draw")), " ");
        a.a.Je && b.push(a.bd = h("button", {
            className: "minw",
            onclick: function() {
                a.na.show(2)
            }
        }, a.g("bl_resign")), " ");
        a.a.ud && b.push(a.cd = h("button", {
            className: "minw",
            onclick: function() {
                X(a, 1, 2)
            }
        }, a.g("bl_undo")), " ");
        a.a.bg && b.push(a.J = h("button", {
            onclick: function() {
                X(a, 1, 10)
            }
        }, a.g("bl_resign") + ": 1"), " ", a.L = h("button", {
            onclick: function() {
                X(a, 1, 11)
            }
        }, "2"), " ", a.N = h("button", {
            onclick: function() {
                X(a,
                    1, 12)
            }
        }, "3"));
        return b
    }

    function ae(a, b) {
        if (a.a.ud || a.a.Ee || a.a.Je) a.na = new Qd(b, {
            className: "trqcont lh1s",
            style: {
                position: "relative"
            }
        }), a.na.add(0, {
            className: "nowrel"
        }, ke(a)), a.na.add(1, {
            className: "trqans dsp1 nowrel",
            style: {
                display: "none"
            }
        }, [h("button", {
            className: "minw",
            onclick: function() {
                X(a, 2, a.eb);
                ie(a, 0)
            }
        }, a.g("bl_yes")), " ", h("button", {
            className: "minw",
            onclick: function() {
                X(a, 3, a.eb);
                ie(a, 0)
            }
        }, a.g("bl_no")), " ", a.sa = h("span", ["..."])]), a.na.add(2, {
            className: "nowrel",
            style: {
                display: "none"
            }
        }, [h("button", {
            className: "minw",
            onclick: function() {
                X(a, 4);
                a.na.show(0)
            }
        }, a.g("bl_yes")), " ", h("button", {
            className: "minw",
            onclick: function() {
                a.na.show(0)
            }
        }, a.g("bl_no")), " ", h("span", {
            className: "ttup"
        }, a.g("bl_resign"))])
    }
    e.Sd = function(a) {
        if (!this.a.wf) {
            var b = this.app.be;
            if (b && !(1 >= b.length)) {
                var c = this;
                h(a, "div", {
                    className: "mbsp"
                }, [h("div", {
                    className: "nowrel"
                }, this.g("tb_tr_add")), this.v = h("select", {
                    onchange: function() {
                        try {
                            Ld(c, "tm", this.options[this.selectedIndex].text)
                        } catch (d) {}
                    }
                }, b.map(function(a) {
                    return h("option", a)
                }))]);
                this.xb.push(this.v)
            }
        }
    };

    function ce(a, b) {
        function c() {
            function a() {
                var a = 0;
                f.forEach(function(b) {
                    b.checked && a++
                });
                d.disabled = !a
            }

            function b() {
                var a = f.filter(function(a) {
                    return a.checked
                }).map(function(a) {
                    return a.value
                });
                0 < a.length && this.send([96, this.F], a)
            }
            var c = this;
            this.Nd || (this.Nd = z(this.app, this.g("t_rpin") || "-", {
                width: "80%",
                minHeight: "0",
                minWidth: "280px",
                maxWidth: "600px"
            }, {
                nopad: !0
            }));
            var d, f = k.map(function(b, c) {
                return h("input", {
                    type: "checkbox",
                    value: b,
                    id: "_chrep" + c,
                    onchange: function() {
                        return a()
                    }
                })
            });
            F(this.Nd.Ca,
                [h("div", {
                    className: "bsbb",
                    style: {
                        width: "100%",
                        height: "220px",
                        padding: "0 15px",
                        borderTop: "solid 1px #ddd",
                        borderBottom: "solid 1px #ddd",
                        overflowY: "scroll"
                    }
                }, 0 == f.length ? h("p", "-") : f.map(function(a) {
                    return h("p", {}, [a, " ", h("label", {
                        htmlFor: a.id
                    }, a.value)])
                })), h("p", {
                    className: "bsbb",
                    style: {
                        padding: "0 15px"
                    }
                }, d = h("button", {
                    disabled: !0,
                    className: "minw",
                    onclick: function() {
                        v(c.app);
                        b.call(c)
                    }
                }, this.g("bl_ok")))]);
            E(this.app, this.Nd)
        }
        var d, f, g = (window.k2prechat || "").split(" ");
        b = h(b, "div", {
            style: {
                position: "absolute",
                width: "100%",
                height: "100%"
            }
        }, [d = h("div", {
            className: "bsbb mb1s",
            style: {
                position: "absolute",
                top: 0,
                bottom: 0,
                left: 0,
                right: 0,
                background: "#fff",
                padding: "2px 4px 3px 8px"
            }
        }), h("div", {
            className: "h1s",
            style: {
                position: "absolute",
                bottom: 0,
                left: 0,
                right: 0,
                paddingTop: "4px"
            }
        }, [h("form", {
            onsubmit: function() {
                a.Y(f.value.trim());
                f.value = "";
                return !1
            }
        }, f = h("input", {
            className: "bsbb",
            name: "somename",
            type: "text",
            autocomplete: "off",
            autocapitalize: "off",
            style: {
                width: "100%",
                border: "none"
            }
        }))]), h("div", {
            className: "bsbb",
            style: {
                position: "absolute",
                top: "100%",
                width: "100%",
                margin: "-2px 0 4px"
            }
        }, [h("button", {
            className: "ddbut butlh",
            onclick: function(b) {
                a.app.G || b.target.focus()
            },
            style: {
                position: "absolute",
                top: 0,
                background: "#f8f8f8",
                border: "none",
                borderRadius: 0
            }
        }, "..."), h("div", {
                className: "ddcont bsbb bs dsp1",
                onmousedown: function(b) {
                    if (a.app.Jf && b.target.onclick) b.target.onclick()
                },
                ontouchend: function(b) {
                    if (a.app.G) {
                        if (b.target.onclick) b.target.onclick();
                        return !1
                    }
                },
                style: {
                    position: "absolute",
                    bottom: 0,
                    width: "100%",
                    background: "#f8f8f8",
                    paddingTop: "1em"
                }
            },
            [h("p", {}, [h("button", {
                onclick: function() {
                    return c.call(a)
                },
                style: {
                    color: "red",
                    borderColor: "red"
                }
            }, a.g("t_rpin"))]), h("p", {}, [h("input", {
                type: "checkbox",
                checked: !0,
                ontouchend: function(b) {
                    a.app.G && (b.target.checked = !b.target.checked, b.target.onchange(b))
                },
                onchange: function(b) {
                    b = b.target.checked;
                    a.Y(b ? "/chat1" : "/chat0");
                    b || a.wa.reset()
                }
            }), " ", a.g("sw_chat")]), h("p", {}, "\ud83d\ude00 \ud83d\ude02 \u2639\ufe0f \ud83e\udd14 \ud83d\ude2d \ud83d\ude34 \ud83e\udd10 \ud83d\udc4d \ud83d\udc4e".split(" ").map(function(b) {
                return h("span", {
                    className: "emo",
                    style: {
                        cursor: "pointer",
                        marginRight: "1px"
                    },
                    onclick: function() {
                        return a.Y(b)
                    }
                }, b)
            })), g ? h("p", {
                style: {}
            }, g.map(function(b) {
                return h("span", {
                    style: {
                        cursor: "pointer",
                        marginRight: ".5em",
                        padding: ".4em 0"
                    },
                    onclick: function() {
                        return a.Y(b)
                    }
                }, b)
            })) : null])])]);
        u(d, {
            wordWrap: "break-word",
            overflowY: "scroll",
            WebkitOverflowScrolling: "touch"
        });
        d.onselectstart = function(a) {
            a.stopPropagation();
            return !0
        };
        var k = [];
        w(a.app, "tabchat", function(a) {
            0 != a.indexOf(N) && k.push(a)
        });
        w(a.app, "tabopen", function(a) {
            0 ==
            a && (k = [])
        });
        var m = a.a.Wf || 0;
        a.wa = {
            b: b,
            append: function(a) {
                for (var b = d.scrollTop + 2 >= d.scrollHeight - d.clientHeight; 0 < m && d.children.length > m && d.firstChild;) d.removeChild(d.firstChild);
                var c = 0 != a.indexOf(N) ? a.indexOf(":") : 0;
                a = h(d, "div", {
                    className: "tind"
                }, 0 < c ? [h("b", [a.substring(0, c)]), a.substring(c)] : a);
                nc(a);
                b && (d.scrollTop = d.scrollHeight - d.clientHeight + 1)
            },
            reset: function() {
                d.innerHTML = "";
                f.value = ""
            },
            Ea: function() {
                d.scrollTop = d.scrollHeight - d.clientHeight + 1
            }
        }
    }

    function de(a, b) {
        var c = h(b.yb, "div", {
            style: {
                position: "absolute",
                width: "100%",
                height: "100%"
            }
        });
        a.lb = new qd(a.app, c, {
            cols: [U, -1],
            Rd: !0,
            dd: function(b, c) {
                1 != c.Ua ? a.app.send([Wa], [b.name]) : (pd(c, 0), a.Y("/boot " + b.name))
            },
            df: {
                className: "mb1s ulwp ovysct"
            }
        });
        b.add(a.g("sw_users"), c);
        h(c, "div", {
            className: "lh1s nowrel",
            style: {
                position: "absolute",
                left: 0,
                right: 0,
                bottom: 0
            }
        }, [a.ad = h("button", {
            className: "minw",
            onclick: function() {
                fe(a)
            }
        }, a.g("bl_invite")), " ", a.Zc = h("button", {
            className: "minw",
            onclick: function() {
                var b =
                        a.lb,
                    c = b.Ua;
                0 != c && 1 != c || pd(b, 1 - b.Ua)
            }
        }, a.g("bl_boot")), null, " ", a.V = h("span", {
            className: "mlh"
        }, ["-"])])
    }

    function le(a, b, c) {
        b = a.u.$d(b, c);
        if (a.history && b) {
            if (Array.isArray(b)) jd(a.history, b);
            else if (c = a.history, 0 != c.a.rows.length) {
                var d = c.a.rows[c.a.rows.length - 1];
                0 != d.cells.length && (F(d.cells[d.cells.length - 1], [id(b)]), c.o.ab && c.Ea(0))
            }
            a.u.da(a.history.f)
        }
    }

    function me(a, b, c) {
        a.u.history(b, c);
        if (a.history && c) {
            b = a.history;
            b.j = 0;
            b.f = -1;
            for (var d = b.a.rows.length - 1; 0 <= d; d--) b.a.deleteRow(d);
            jd(b, c);
            a.u.da(a.history.f)
        }
    }

    function hd(a, b) {
        a.u.Eb || (ld(a.history, b), a.u.da(a.history.f))
    }

    function ee(a, b) {
        var c = h(b.yb, "div", {
                className: "bsbb dsp1",
                style: {
                    background: "#fff",
                    position: "absolute",
                    top: "0",
                    width: "100%",
                    height: "100%"
                }
            }),
            d = h(c, "div", {
                style: {
                    width: "50%",
                    cssFloat: "left",
                    marginTop: ".75em"
                }
            }),
            f = h(d, "div", {
                className: "mbsp"
            }),
            g = [];
        g.push(a.g("tb_ttpub"));
        if (a.app.ea)
            for (var k = 0; 7 > k; k++) g.push(J(a.app, 0 == k % 2 ? Ka(a.app, 1 + (k >> 1)) : Math.floor((Ka(a.app, 1 + (k >> 1)) + Ka(a.app, (k >> 1) + 2)) / 2)) + "+");
        g.push(a.g("tb_ttprv"));
        a.P = h(f, "select", {
            onchange: function() {
                var b = this.selectedIndex;
                Ld(a, "ttype",
                    a.app.ea && 0 < b ? 8 <= b ? 2 : b + 2 : 2 * b)
            }
        }, g.map(function(a) {
            return h("option", a)
        }));
        f = h(d, "div", {
            className: "mbsp"
        });
        h(f, "div", {}, a.g("tb_tr_game"));
        a.xb.push(a.G = h(f, "select", {
            onchange: function() {
                try {
                    Ld(a, "tg", a.a.Zf ? -(this.selectedIndex + 1) : this.options[this.selectedIndex].text)
                } catch (m) {}
            }
        }, a.app.de.map(function(a) {
            return h("option", a)
        })));
        a.a.wf && a.xb.push(a.v = h(f, "select", {
            onchange: function() {
                try {
                    Ld(a, "tm", this.options[this.selectedIndex].text)
                } catch (m) {}
            }
        }, a.app.je.map(function(a) {
            return h("option", a)
        })));
        a.a.ud && "undefined" == typeof a.a.ag && (h(d, "div", {
            className: "mbsp"
        }, [a.B = h("input", {
            type: "checkbox",
            onchange: function() {
                Ld(a, "ud", this.checked ? 1 : 0)
            }
        }), a.g("tb_noundo")]), a.xb.push(a.B));
        h(d, "div", {}, [h("input", {
            type: "checkbox",
            checked: a.app.ta,
            onchange: function() {
                var b = this.checked,
                    c = a.app;
                c.ta = b;
                Ga(c);
                b && ne(a.app.ta)
            }
        }), a.g("p_bp")]);
        d = h(c, "div", {
            style: {
                width: "50%",
                cssFloat: "right",
                marginTop: ".75em"
            }
        });
        h(d, "div", {
            className: "mbsp nowrel"
        }, [a.j = h("input", {
            type: "checkbox",
            onchange: function() {
                Ld(a, "gtype",
                    this.checked ? 0 : 1)
            }
        }), a.g("tb_gtnrt") + " (x)"]);
        a.Sd(d);
        !a.app.S && a.history && h(d, "div", h("button", {
            onclick: function() {
                a.f.show(a.ea)
            }
        }, [a.g("sw_history")]));
        b.add(a.g("sw_setup"), c, !0)
    }

    function oe(a, b, c) {
        for (var d = 0; d < c.length - 2; d++) {
            var f = parseInt(c[d + 2], 10);
            "ttype" == b[d] ? a.P.selectedIndex = a.app.ea && 1 < f ? 2 == f ? 8 : f - 2 : f >> 1 : "gtype" == b[d] ? a.$b || (a.j.checked = 0 == f) : "tm" == b[d] ? a.v && pe(a.v, f.toString()) : "tg" == b[d] ? a.G && (0 > f ? a.G.selectedIndex = -f - 1 : pe(a.G, f.toString())) : "ud" == b[d] ? a.B && (a.B.checked = 0 != f) : "tch" != b[d] && "op:" == b[d].substring(0, 3) && a.V && F(a.V, ["op: " + b[d].substring(3)])
        }
    }

    function Xd(a) {
        return 0 >= a.F ? "-" : mc(a.g("l_tab"), a.F.toString()) + " \u00a0 " + a.pa.O[0]
    }

    function Ud(a) {
        W(a) && 0 != (a.oa & 64) && (V(a) && 0 != (a.U & 2) || 0 != (a.oa & 16) && 0 == (a.U & 16)) || (a.app.send([73, a.F], null), y(a.app, "tabopen", 0), $a(a.app), a.F = -1, setTimeout(function() {
            -1 == a.F && a.reset()
        }, 50))
    }

    function Bb(a, b, c) {
        0 < a.F && a.reset();
        2 < b.length && (a.qd = Math.floor(b[2] / 16) % 3, a.$b = 0 != a.qd, a.j.checked = 1 == a.qd, a.j.disabled = a.$b);
        a.pa = new Ad(a.app, b, c);
        a.F = a.pa.K[1];
        y(a.app, "tabopen", a.F);
        a.qc = !0;
        window.k2spectm && a.wa.append(N + window.k2spectm);
        a.app.S && a.xa && a.wa.append(N + "(info) " + a.xa)
    }

    function Vd(a, b) {
        q(a.b, "sbdropvis", b)
    }
    e.reset = function() {
        if (0 != this.F) {
            this.F = 0;
            y(this.app, "tabopen", 0);
            this.qc = !0;
            this.Vc = !1;
            this.Ec = null;
            var a = this.hb;
            a.A && (clearInterval(a.A), a.A = 0, a.I = !1);
            a.a = -1;
            a.v = 0;
            Vd(this, !1);
            this.T.reset();
            qe(this);
            this.wa.reset();
            this.history && this.history.reset();
            this.lb.reset();
            this.na && this.na.show(0);
            this.f && this.f.show(0);
            he(this, null);
            re(this, !1, !1);
            this.u.reset(!0);
            Q(this.u)
        }
    };

    function se(a, b, c, d) {
        1 == c ? (a = a.T, a.X && (a.X[b] = d.toString())) : 2 == c && Id(a.T, b, d)
    }

    function te(a, b) {
        b != a.hf && (a.hf = b, 8 != b ? V(a) && (b = 9 == b ? a.g("win_draw") : 0 > b ? mc(a.g("los_pln"), (-b).toString()) : mc(a.g("win_pln"), (b + 1).toString()), a.Ec = b, a.wa.append(N + b)) : a.Ec = null)
    }

    function $d(a, b) {
        for (var c = a.T, d = h("div", {
            className: "tplcont",
            style: {
                overflowY: "auto"
            }
        }), f = 0; f < c.ca; f++) {
            var g = h(d, "div", {
                    style: {
                        cssFloat: 0 == f % 2 ? "left" : "right",
                        width: "49.5%",
                        overflowX: "hidden",
                        marginTop: 2 <= f ? a.app.S ? "8px" : "4px" : 0
                    }
                }),
                k = h(g, "div", {
                    className: "f12",
                    style: {
                        verticalAlign: "middle",
                        lineHeight: "12px",
                        background: "rgba(0,0,0,0.8)",
                        color: "rgba(255,255,255,0.95)",
                        fontWeight: "bold",
                        padding: "0 5px"
                    }
                });
            c.Kb && h(k, "div", {
                style: {
                    display: "inline-block",
                    width: "7px",
                    height: "7px",
                    background: c.Kb[(f + (c.xd ?
                        1 : 0)) % c.ca],
                    marginRight: "5px"
                }
            });
            h(k, ["#" + (f + 1)]);
            !a.a.Md && c.X && h(k, "div", {
                style: {
                    display: "inline-block",
                    marginLeft: "4px",
                    width: 0,
                    height: 0,
                    borderLeft: "solid 4px transparent",
                    borderRight: "solid 4px transparent",
                    borderTop: "solid 7px rgba(255,255,255,0.9)",
                    visibility: a.Ya == f ? "inherit" : "hidden"
                }
            });
            var m = h(g, "div", {
                    style: {
                        position: "relative"
                    }
                }),
                l = c.a[f];
            k = 0 == l;
            h(m, "button", {
                className: "butsys butsit",
                disabled: 1 != l,
                style: {
                    display: k ? "none" : "block",
                    position: "absolute",
                    width: "100%",
                    height: "100%"
                },
                onclick: function(b) {
                    return function() {
                        a.send([83,
                            a.F, b
                        ], null)
                    }
                }(f)
            }, ["#" + (f + 1)]);
            m = h(m, "div", {
                style: {
                    background: "#fff",
                    padding: "6px 6px 6px 0",
                    visibility: k ? "inherit" : "hidden"
                }
            });
            h(m, "button", {
                style: {
                    display: 0 != (c.xc & 1 << f) && null != c.name[f] ? "block" : "none",
                    cssFloat: "right",
                    border: 0,
                    padding: "2px 6px",
                    fontWeight: "bold",
                    margin: "1px 0",
                    background: "#bbb",
                    color: "#fff"
                },
                onclick: function(b) {
                    return function() {
                        a.send([84, a.F, b], null)
                    }
                }(f)
            }, ["X"]);
            h(m, "div", {
                className: "nowrel",
                style: {
                    fontSize: "115%",
                    color: c.focus[f] ? "inherit" : "#aaa",
                    padding: "2px 6px"
                }
            }, [k ? c.name[f] ||
                "--" : "-"
            ]);
            g = h(g, "div", {
                className: "tplext",
                style: {
                    marginTop: "6px",
                    width: "100%"
                }
            });
            c.X && c.kc && h(g, "div", {
                style: {
                    display: "table-cell",
                    width: "52%",
                    textAlign: "center"
                }
            }, h("div", {
                style: {
                    background: "#fff",
                    opacity: ".7",
                    fontWeight: "bold",
                    padding: "1px 0 2px"
                }
            }, [c.X[f]]));
            c.time && h(g, "div", {
                style: {
                    display: "table-cell",
                    verticalAlign: "middle",
                    textAlign: c.kc ? "center" : "left"
                }
            }, [c.time[f], a.a.Md || c.X ? null : h("div", {
                style: {
                    marginLeft: "8px",
                    display: "inline-block",
                    width: 0,
                    height: 0,
                    borderLeft: "4px solid transparent",
                    borderRight: "4px solid transparent",
                    borderBottom: "solid 8px rgba(0,0,0,0.8)",
                    visibility: f == a.Ya ? "inherit" : "hidden"
                }
            })]);
            c.time || h(g, "div", {
                style: {
                    display: "table-cell"
                }
            })
        }
        F(b, d)
    }

    function qe(a) {
        y(a.app, "tabplayers")
    }

    function ue(a) {
        for (var b = 0; b < a.T.ca; b++)
            if (b >= a.pa.Ld) {
                var c = a.T,
                    d = b;
                c.name[d] = null;
                c.a[d] = 0
            } else if (c = a.pa.K[wd + b], 1 == c) c = a.T, d = b, c.name[d] = a.pa.O[vd + b], c.a[d] = 0;
            else if (3 == c) c = a.T, d = b, c.name[d] = null, c.a[d] = 0;
            else {
                d = a.T;
                var f = b;
                d.a[f] = 0 == c && Kd(a) ? 1 : -1;
                d.name[f] = ""
            }
        ve(a);
        qe(a)
    }

    function Hb(a, b, c) {
        Dd(a.pa, b, c);
        y(a.app, "tabstatus");
        ue(a);
        a.Ta && Q(a.u)
    }

    function re(a, b, c) {
        a.u.setActive(b, !0);
        c && ne(a.app.ta);
        b = b || a.Vc;
        y(a.app, "tabalert", b);
        a = a.app;
        b = b ? "\u25bc" : null;
        c = "hasFocus" in document ? document.hasFocus() : !0;
        null != b && (null == b || c && Za(a)) || a.Xb == b || (a.Xb = b, Xa(a))
    }

    function we(a) {
        var b = a.Ya,
            c = a.zb,
            d = 0;
        V(a) && 0 != (a.U & 2) || 0 != (a.oa & 16) && 0 == (a.U & 16) || (d = 0 == (a.oa & 1) || V(a) && 0 == (a.U & 8) ? W(a) ? 1 << c : 0 : 15);
        a.T.xc = d;
        d = W(a) && V(a) && 0 == (a.U & 8);
        a.bd && (a.bd.disabled = !d);
        a.J && (a.J.disabled = !d);
        a.L && (a.L.disabled = !d);
        a.N && (a.N.disabled = !d);
        d = W(a) && V(a) && 0 == (a.U & 4) && 0 == (a.U & 8);
        a.$c && (a.$c.disabled = !(d && b != c));
        a.cd && (a.cd.disabled = !(d && !a.ef));
        var f = 0 != (a.oa & 1);
        a.P.disabled = !f;
        a.ad && (a.ad.disabled = !f);
        a.Zc && (a.Zc.disabled = !f);
        f = f && !V(a);
        a.$b || (a.j.disabled = !f);
        for (var g = 0; g < a.xb.length; g++) a.xb[g].disabled = !f;
        ue(a);
        b = d && b == c && -1 != b;
        re(a, b, b && !a.qe);
        a.qe = b;
        Q(a.u)
    }
    e.Be = function(a, b) {
        return b
    };

    function xe(a, b, c, d) {
        if (1 == b[c] || 2 == b[c]) {
            var f = 2 == b[c];
            if (c + (3 + (f ? 1 : 0)) > d) return c;
            d = b[c + 1];
            var g = b[c + 2] % 2,
                k = a.Ya;
            0 > d && (d += 65536);
            a.Bb = -1 > b[c + 2] ? "0:00" : null; - 1 > b[c + 2] && (k = -b[c + 2] >> 2);
            a = a.hb;
            b = f ? b[c + 3] : 0;
            a.j = Date.now();
            a.a = k;
            a.J = d;
            a.f = "undefined" != typeof b ? b : 0;
            a.B = g;
            a.I || a.start();
            0 < a.f && (a.G = -1, ye(a));
            c += 3 + (f ? 1 : 0)
        } else if (3 == b[c]) {
            if (c + 2 > d) return c;
            a.ef = 0 != (b[c + 1] & 1);
            f = 0 != (b[c + 1] & 2);
            d = a.T;
            d.xd != f && (d.xd = f);
            a.u.bf(f);
            f = b[c + 1] >> 3;
            for (b = 0; b < a.T.ca; b++) a.T.focus[b] = 0 == (f & 1 << b);
            c += 2
        }
        return c
    }

    function ob(a, b, c) {
        if (!(2 > b.length)) switch (b[0]) {
            case 92:
                le(a, b, c);
                break;
            case 91:
                a.qc = !1;
                me(a, b.slice(2), c);
                break;
            case 88:
                if (5 > b.length) break;
                var d = a.zb;
                a.oa = b[2];
                a.U = b[3];
                a.zb = b[4];
                a.u.yd(a.zb);
                a.Vc = W(a) && 0 != (a.U & 8) && 0 == (a.oa & 8) && 0 == (a.U & 16);
                we(a);
                a.zb != d && a.history && !md(a.history) && (jd(a.history, []), a.u.da(a.history.f));
                break;
            case 90:
                if (3 > b.length) break;
                if (b.length < b[2] + 4) break;
                a.hb && (a.hb.a = -1);
                c = b[3 + b[2]];
                if (0 == c) break;
                var f = b[2] + 4,
                    g = f + c,
                    k = 0;
                if (b.length < f + c) break;
                for (; b.length >= g + c;) {
                    for (d = 0; d <
                    c; d++) se(a, k, b[f + d], b[g + d]);
                    g += c;
                    k++
                }
                0 < b[2] && (a.Ya = b[3]);
                1 < b[2] && te(a, b[4]);
                a.Bb = null;
                d = 3 + b[2];
                for (c = 5; c < d;) {
                    f = 5 > b[c] ? xe(a, b, c, d) : a.Be(b, c, d);
                    if (c == f) break;
                    c = f
                }
                we(a);
                break;
            case 93:
                if (3 > b.length) break;
                0 < c.length ? ie(a, b[2], c) : ie(a, 0);
                break;
            case 81:
                if (1 > c.length) break;
                y(a.app, "tabchat", c[0]);
                a.wa.append(c[0]);
                a.f && 2 < b.length && a.f.show(0);
                break;
            case 89:
                if (2 > b.length || c.length < b.length - 2) break;
                oe(a, c, b);
                break;
            case 87:
                if (3 > b.length) break;
                a.T.focus[b[2] >> 1] = 0 != (b[2] & 1);
                qe(a);
                break;
            case 94:
                if (1 > c.length) break;
                a.A || (a.A = z(a.app, a.a.cc || "-", {
                    width: "80%",
                    minHeight: "0",
                    minWidth: "280px",
                    maxWidth: "600px"
                }, {
                    nopad: !0
                }), a.A.Ca.onselectstart = function(a) {
                    a.stopPropagation();
                    return !0
                });
                F(a.A.Ca, h("textarea", {
                    value: c[0],
                    className: "bsbb taplain",
                    style: {
                        width: "100%",
                        height: "280px",
                        borderTop: "solid 1px #ddd",
                        padding: "4px 15px",
                        fontFamily: "monospace"
                    },
                    spellcheck: !1,
                    readOnly: !0
                }));
                E(a.app, a.A, null, {
                    okselect: !0
                });
                break;
            case 84:
                if (1 > c.length) break;
                b = cb(a.app, c[0]);
                null != b && Rb(a.lb, b);
                0 == (a.oa & 1) && !W(a) || V(a) || ne(a.app.ta);
                break;
            case 85:
                if (1 > c.length) break;
                b = cb(a.app, c[0]);
                null != b && ze(a.lb, b);
                break;
            case 86:
                f = {};
                for (d = 0; d < c.length; d++) b = cb(a.app, c[d]), null == b || f.hasOwnProperty(b.name) || (f[b.name] = b);
                eb(a.lb, f)
        }
    }

    function ve(a) {
        var b = null,
            c = !1,
            d = !1;
        0 != (a.U & 16) ? b = "aw_rnd" : 0 != (a.U & 4) ? Kd(a) ? b = "pr_sel" : V(a) ? b = "aw_pls" : (b = "aw_opp", d = !0) : 0 != (a.U & 8) && (W(a) && 0 == (a.oa & 8) ? (b = "", c = !0) : b = "aw_go");
        if (null != b) {
            b = "" != b ? a.g(b) : "-";
            var f;
            d && (xd(a.pa) ? 0 < (f = yd(a.pa)) && (b += " (" + J(a.app, f) + "+)") : b += " (" + a.g("bl_invite") + ")");
            null != a.Bb && (b = "(" + a.Bb + ")" + ("" != b && "-" != b ? " " + b : ""));
            he(a, b, c)
        } else he(a, null)
    }

    function je(a, b, c) {
        switch (b) {
            case 1:
                return a.g("bl_draw");
            case 2:
                return a.g("bl_undo");
            case 4:
                return -1 != c.indexOf("/") ? c : a.g("bl_tram");
            case 10:
                return a.g("bl_resign") + " 1";
            case 11:
                return a.g("bl_resign") + " 2"
        }
        return "(?)"
    }
    e.Pb = function(a) {
        a && (a = this.app, this.Pe = !!a.B && (!a.Fc || a.B != a.Fc.b))
    };
    e.onshow = function() {
        var a = parseInt(Ma(), 10);
        a != this.F && ab(this.app, a, !0);
        this.wa && this.wa.Ea()
    };

    function Ad(a, b, c) {
        this.app = a;
        b[2] %= 16;
        this.K = b;
        this.O = c;
        for (a = this.O.length - 1; 1 <= a; a--) b = this.O[a].indexOf("/"), 0 <= b && (this.O[a] = this.O[a].substring(0, b));
        this.F = this.K[1].toString();
        this.Ld = this.K.length - wd
    }
    var wd = 3,
        vd = 1;

    function yd(a) {
        var b = 1 + Math.floor((a.K[2] - 3) / 2);
        return 2 < a.K[2] ? 0 == (a.K[2] - 3) % 2 ? Ka(a.app, b) : Ka(a.app, b) + Ka(a.app, b + 1) >> 1 : 0
    }

    function Ed(a) {
        for (var b = 0, c = wd; c < a.K.length; c++) 0 == a.K[c] && b++;
        return b
    }

    function Fd(a) {
        for (var b = 0, c = wd; c < a.K.length; c++) 1 != a.K[c] && 2 != a.K[c] || b++;
        return b
    }

    function Gd(a, b) {
        return 0 == a.K[2] || 3 <= a.K[2] && b >= yd(a)
    }

    function xd(a) {
        return 0 == a.K[2] || 3 <= a.K[2]
    }

    function Dd(a, b, c) {
        if (!(3 > b.length)) {
            b[2] %= 16;
            a.K = b;
            a.O = c;
            for (c = a.O.length - 1; 1 <= c; c--) {
                var d = a.O[c].indexOf("/");
                0 <= d && (a.O[c] = a.O[c].substring(0, d))
            }
            a.Ld = b.length - wd
        }
    }

    function Qb(a, b, c, d) {
        var f = 0;
        this.name = b;
        this.a = d[f++];
        this.j = a.Qc && a.Qc[Math.floor(this.a / Ae)] || "";
        this.table = 0 != (c & Be) && f < d.length ? d[f++] : 0;
        this.X = 0 != (c & rd) && f < d.length ? d[f++] : 0;
        this.f = 0 != (c & U) && f < d.length ? d[f++] : 0;
        this.Ra = []
    }
    var Be = 1,
        rd = 2,
        U = 4,
        T = 16,
        kc = 1,
        lc = 2,
        Ae = 16;

    function Ce(a, b) {
        b = a.Ra.indexOf(b); - 1 != b && a.Ra.splice(b, 1)
    }

    function jb(a) {
        for (; 0 < a.Ra.length;) ze(a.Ra[0], a)
    }

    function De(a, b) {
        switch (b) {
            case Be:
                return a.table;
            case U:
                return a.f;
            case rd:
                return a.X;
            case T:
                return 0 == a.table ? 1 : 0
        }
        return 0
    }

    function Pb(a, b, c) {
        var d = 0;
        a.a = c[d++];
        a.table = 0 != (b & Be) && d < c.length ? c[d++] : 0;
        a.X = 0 != (b & rd) && d < c.length ? c[d++] : 0;
        a.f = 0 != (b & U) && d < c.length ? c[d++] : 0;
        for (b = a.Ra.length - 1; 0 <= b; b--)
            if (c = a.Ra[b], d = a, c.f.hasOwnProperty(d.name)) {
                var f = c.f[d.name];
                f.C = Ee(c, d, f.C);
                f = c.j.indexOf(d); - 1 != f && (c.j.splice(f, 1), Fe(c, d, c.j.length))
            }
    }

    function qd(a, b, c) {
        this.app = a;
        this.o = c;
        this.Ua = 0;
        this.cols = c.cols;
        this.v = c.Pf || 0;
        this.B = 0 == this.v;
        this.f = {};
        this.j = [];
        this.b = h(b, "div", c.df || {});
        this.a = h(this.b, "table", {
            className: "ul " + (c.pf ? "uls2" : "uls1")
        });
        this.A = c.Ke ? sd(this, null) : null
    }

    function td(a, b) {
        if (a.v != b) {
            a.v = b;
            a.B = 0 == b;
            b = 1;
            for (var c = a.j.length; b < c; b++) {
                var d = a.j[b];
                a.j.splice(b, 1);
                Fe(a, d, b)
            }
        }
    }

    function pd(a, b) {
        a.Ua != b && (a.Ua = b, q(a.a, "ulm1", 1 == b))
    }
    qd.prototype.reset = function() {
        this.j.length = 0;
        Object.keys(this.f).forEach(function(a) {
            var b = this.f[a];
            Ce(b.$a, this);
            b.C && (b = b.C, b.parentNode.removeChild(b));
            delete this.f[a]
        }, this);
        pd(this, 0)
    };

    function Fe(a, b, c) {
        for (var d = a.v; 0 < c;) {
            var f = a.j[c - 1];
            if (0 != d) {
                var g = De(f, d);
                var k = De(b, d);
                g = g < k ? -1 : g > k ? 1 : 0
            } else g = f.name < b.name ? -1 : f.name > b.name ? 1 : 0;
            a.B && (g = -g);
            if (0 > g || 0 == g && 0 < (f.name < b.name ? -1 : f.name > b.name ? 1 : 0)) c--;
            else break
        }
        d = (d = a.j[c]) ? a.f[d.name] : null;
        a.j.splice(c, 0, b);
        a = a.f[b.name];
        a.C && (a = a.C, a.parentNode.insertBefore(a, d ? d.C : null))
    }

    function Rb(a, b) {
        a.f.hasOwnProperty(b.name) || (a.f[b.name] = {
            $a: b
        }, a.f[b.name].C = Ee(a, b), Fe(a, b, a.j.length), b.Ra.push(a))
    }

    function ze(a, b) {
        if (a.f.hasOwnProperty(b.name)) {
            Ce(b, a);
            var c = a.f[b.name];
            c.C && c.C.parentNode && (c = c.C, c.parentNode.removeChild(c));
            c = a.j.indexOf(b); - 1 != c && a.j.splice(c, 1);
            delete a.f[b.name]
        }
    }

    function eb(a, b) {
        a.reset();
        Object.keys(b || {}).forEach(function(a) {
            a = b[a];
            this.f[a.name] = {
                $a: a
            };
            this.f[a.name].C = Ee(this, a);
            Fe(this, a, this.j.length);
            a.Ra.push(this)
        }, a)
    }

    function sd(a, b) {
        var c = h("tr", {
            className: "ulhead"
        });
        h(c, "td", {
            onclick: function() {
                td(a, 0);
                return !1
            }
        }, h("div", {
            className: "darr"
        }));
        for (var d = 0; d < a.cols.length; d++) h(c, "td", {
            onclick: function(b) {
                return function() {
                    td(a, a.cols[b])
                }
            }(d)
        }, h("div", {
            className: "darr"
        }));
        zd(a.a, c, b);
        return c
    }

    function Ee(a, b, c) {
        var d = a.o.Rd && b.j != "(" + a.app.lang + ")";
        d = h("tr", {
            onclick: function() {
                a.o.dd(b, a);
                return !1
            }
        }, [h("td", {}, [0 != (b.a & kc) || 0 != (b.a & lc) ? h("div", {
            className: "ulsym",
            style: {
                cssFloat: "right"
            }
        }, 0 != (b.a & lc) ? "X" : 0 != (b.a & kc) ? "\u2605" : "") : null, h("div", {
            className: "ulnm"
        }, [h("div", {
            className: "r" + La(a.app, b.f)
        }), b.name, h("span", {
            className: "ulla" + (d ? "" : " ulla0")
        }, b.j)])]), h("td", {
            className: "m1ac"
        }, h("button", {
            className: "ulbx"
        }, "X"))]);
        for (var f = 0; 2 > f; f++) {
            var g = a.cols[f],
                k = {
                    className: "m0ac ulnu"
                };
            g ==
            Be ? h(d, "td", k, Ia(b.table)) : g == rd ? h(d, "td", k, Ja(a.app, b.X)) : g == U ? h(d, "td", k, J(a.app, b.f)) : g == T && h(d, "td", {
                className: "m0ac ulnu",
                title: Ia(b.table)
            }, 0 != b.table ? "#" : "")
        }
        zd(a.a, d, c);
        return d
    }
    var Y = null,
        Z = null,
        Ge = null,
        He = !1,
        Ie = !1;

    function Td(a) {
        function b() {
            Ge = Z.createBuffer(1, 2048, Z.sampleRate);
            for (var a = Ge.getChannelData(0), b = 0, c = a.length; b < c; b++) a[b] = .6 * Math.sin(b * Math.PI * 2 / 52) * (c - b) / c
        }
        a && Je(a);
        a = window.navigator.userAgent || "";
        var c = window.AudioContext || window.webkitAudioContext;
        if (c) {
            try {
                Z = new c
            } catch (d) {
                return
            }
            0 < a.indexOf("Windows NT 5.1") && 0 < a.indexOf("Firefox/") ? b() : (a = window.k2snd["a.mp3"], Z.decodeAudioData(function(a) {
                var b = a.length / 4 * 3;
                a = window.atob(a);
                for (var c = new ArrayBuffer(b), d = new Uint8Array(c), m = 0; m < b; m++) d[m] =
                    a.charCodeAt(m);
                return c
            }(a.substring(a.indexOf(",") + 1)), function(a) {
                Ge = a
            }, function() {
                b()
            }))
        }
    }

    function ne(a) {
        if (Z) Ge && a && (a = Z.createBufferSource(), a.buffer = Ge, a.connect(Z.destination), a.noteOn ? a.noteOn(0) : a.start(0));
        else if (He || Ke(), Y && a) try {
            Y.play()
        } catch (b) {}
    }

    function Ke() {
        if (Y = document.createElement("audio")) 0 < (window.navigator.userAgent || "").indexOf(" Firefox/4") || !Y.canPlayType || !Y.canPlayType("audio/mpeg") ? Y = null : Y.src = window.k2snd["a.mp3"];
        He = !0
    }

    function Je(a) {
        if (!Ie) {
            Ie = !0;
            var b = function() {
                document.removeEventListener(a, b, !0);
                if (Z) {
                    var c = Z.createBufferSource();
                    try {
                        c.buffer = Z.createBuffer(1, 1, Z.sampleRate)
                    } catch (d) {}
                    c.connect(Z.destination);
                    c.noteOn ? c.noteOn(0) : c.start(0)
                } else if (He || Ke(), Y) try {
                    Y.play(), Y.pause()
                } catch (d) {}
            };
            document.addEventListener(a, b, !0)
        }
    }

    function na(a) {
        var b = this;
        this.o = a;
        this.j = this.f = 0;
        this.L = -1;
        this.J = "";
        this.a = 0;
        this.N = this.G = null;
        this.A = "";
        this.I = 0;
        setTimeout(function() {
            (0 < b.f || 0 == b.f && 1 == b.L) && b.o.pd(ra)
        }, 500);
        this.P = Date.now();
        Le(this)
    }
    var ra = -1,
        oa = 1,
        pa = 2;

    function Le(a) {
        a.f += 1;
        if (a.f > a.o.ports.length) a.f = -1, a.o.pd(oa);
        else {
            var b = a.f;
            a.j = setTimeout(function() {
                a.j = 0;
                Me(a, b)
            }, 1E3 * (2 == b ? 2 : 6));
            var c = a.o.ports[b - 1].split(":");
            2 > c.length || ("wss" == c[0] || "ws" == c[0] ? Ne(a, b, c[0], parseInt(c[1], 10)) : ("https" == c[0] || "http" == c[0]) && Oe(a, b, c[0], parseInt(c[1], 10)))
        }
    }

    function Pe(a, b) {
        if (b != a.f) return !1;
        a.j && (clearTimeout(a.j), a.j = 0);
        a.f = 0;
        return !0
    }

    function Me(a, b) {
        b == a.f && (a.j && (clearTimeout(a.j), a.j = 0), Le(a))
    }

    function Qe(a) {
        a.B && (clearTimeout(a.B), a.B = null);
        a.o.pd(pa)
    }

    function Re(a, b) {
        b = b.split("\n");
        for (var c = 0, d = b.length; c < d; c++) {
            try {
                var f = JSON.parse(b[c]);
                var g = f.i || [];
                var k = f.s || []
            } catch (m) {
                fa("PARSE/ds.l=" + b.length + " >" + b[c] + "< " + m);
                return
            }
            0 != g.length && (-1 == a.L && (a.L = 1 == g[0] ? 1 : 0), 1 == g[0] ? a.o.ze() && a.send([2], null) : a.o.tf(g, k))
        }
        a.B || (a.B = setTimeout(function() {
            return Se(a)
        }, 3E4))
    }

    function Se(a) {
        a.o.ze() && a.send([], null);
        a.B = setTimeout(function() {
            return Se(a)
        }, 3E4)
    }

    function Te(a, b) {
        b = b ? b : [];
        for (var c = 0, d = b.length; c < d; c++) b[c] = '"' + b[c].replace(/\\/g, "\\\\").replace(/"/g, '\\"') + '"';
        return '{"i":[' + a.join() + "]" + (0 < b.length ? ',"s":[' + b.join() + "]" : "") + "}"
    }
    na.prototype.send = function(a, b) {
        if (this.v || this.a)
            if (a = Te(a, b), this.v) try {
                this.v.send(a)
            } catch (c) {} else this.a && Ue(this, a)
    };

    function Ne(a, b, c, d) {
        try {
            var f = new WebSocket(c + "://" + a.o.host + ":" + d + "/ws/")
        } catch (g) {
            Me(a, b);
            return
        }
        f.onclose = function() {
            Me(a, b)
        };
        f.onopen = function() {
            f.onmessage = function(c) {
                Pe(a, b) ? (a.v = f, a.v.onmessage = function(b) {
                    window.location.href = "uniwebview://in?data=" + JSON.stringify(b.data);
                    Re(a, b.data)
                }, a.v.onclose = function() {
                    a.v = null;
                    Qe(a)
                }, Re(a, c.data)) : f.close()
            };
            var c = a.o.ye(b, a.P);
            f.send(Te(c.K, c.O))
            window.location.href = "uniwebview://out?data=" + JSON.stringify(Te(c.K, c.O));
            window.location.href = "uniwebview://out2?data=" + JSON.stringify(c);
        }
    }

    function Oe(a, b, c, d) {
        a.J = c + "://" + a.o.host + ":" + d;
        new Ve({
            url: a.J + "/r/0",
            data: "1",
            cb: function() {
                Me(a, b)
            },
            onload: function(c) {
                Pe(a, b) && (a.a = c || "X" + Math.random(), a.I = 0, c = a.o.ye(b, a.P), a.send(c.K, c.O))
            }
        })
    }

    function We(a) {
        a.a && !a.G && (a.G = new Ve({
            url: a.J + "/r/" + a.a,
            data: null,
            cb: function() {
                a.G = null;
                0 < a.I ? a.a && (a.a = null, Qe(a)) : (a.I++, setTimeout(function() {
                    We(a)
                }, 25))
            },
            onload: function(b) {
                a.G = null;
                a.I = 0;
                b && 0 < b.length && Re(a, b);
                setTimeout(function() {
                    We(a)
                }, 25)
            }
        }))
    }

    function Ue(a, b) {
        a.a && (b && 0 < b.length && (a.A += (0 < a.A.length ? "\n" : "") + b), a.N || (b = a.A, a.A = "", a.N = new Ve({
            url: a.J + "/w/" + a.a,
            data: b,
            cb: function() {
                a.a && (a.a = null, Qe(a))
            },
            onload: function() {
                a.G || We(a);
                a.N = null;
                setTimeout(function() {
                    0 < a.A.length && Ue(a)
                }, 25)
            }
        })))
    }

    function Ve(a) {
        setTimeout(function() {
            var b = new XMLHttpRequest;
            a.od || "withCredentials" in b ? b.onreadystatechange = function() {
                if (4 == b.readyState)
                    if (200 == b.status || 204 == b.status) {
                        if (a.onload) a.onload(b.responseText)
                    } else a.cb && a.cb(b.status)
            } : window.XDomainRequest && (b = new XDomainRequest, b.onprogress = function() {}, b.onload = function() {
                if (a.onload) a.onload(b.responseText)
            }, b.onerror = b.ontimeout = function() {
                a.cb && a.cb()
            });
            b.open("POST", a.url, !0);
            a.od && !a.uf && b.setRequestHeader && b.setRequestHeader("Content-type",
                "application/x-www-form-urlencoded");
            b.send(a.data)
        }, 0)
    }

    function Xe(a, b) {
        var c = this;
        new Ve({
            url: a,
            data: b,
            od: !0,
            uf: "string" !== typeof b,
            onload: function(a) {
                c.handle && c.handle(!0, JSON.parse(a))
            },
            cb: function() {
                c.handle && c.handle(!1, {})
            }
        })
    }

    function h(a) {
        var b = arguments,
            c = null,
            d = null,
            f = 0;
        if (0 == b.length) return c;
        b[f].nodeName && (d = b[f++]);
        if (Array.isArray(b[f])) return c = [], b[f].forEach(function(a) {
            null != a && (a = "string" == typeof a || "number" == typeof a ? document.createTextNode(a) : a, d && d.appendChild(a), c.push(a))
        }), c;
        if ("string" !== typeof b[f]) return c;
        c = document.createElement(b[f++]);
        if (f < b.length && "object" == typeof b[f] && !Array.isArray(b[f]) && !b[f].nodeName) {
            var g = b[f++];
            Object.keys(g).forEach(function(a) {
                "style" == a ? Object.keys(g[a]).forEach(function(b) {
                    c.style[b] =
                        g[a][b]
                }) : c[a] = g[a]
            })
        }
        d && d.appendChild(c);
        if (f >= b.length) return c;
        b = b[f];
        "string" == typeof b ? c.appendChild(document.createTextNode(b)) : Array.isArray(b) ? b.forEach(function(a) {
            "string" == typeof a ? c.appendChild(document.createTextNode(a)) : a && c.appendChild(a)
        }) : c.appendChild(b);
        return c
    }

    function F(a, b) {
        a.innerHTML = "";
        Array.isArray(b) ? b.forEach(function(b) {
            "string" == typeof b || "number" == typeof b ? a.appendChild(document.createTextNode(b)) : b && a.appendChild(b)
        }) : a.appendChild(b)
    }

    function bd(a, b, c) {
        return c ? a.insertBefore(b, c) : a.appendChild(b)
    }

    function zd(a, b, c) {
        c ? a.replaceChild(b, c) : a.appendChild(b);
        return b
    }

    function pe(a, b) {
        for (var c = 0; c < a.options.length; c++)
            if (a.options[c].text == b) {
                a.selectedIndex = c;
                break
            }
    }

    function u(a, b) {
        "undefined" !== typeof b && Object.keys(b).forEach(function(c) {
            a.style[c] = b[c]
        })
    }

    function q(a, b, c) {
        2 == arguments.length || c ? a.classList.add(b) : a.classList.remove(b)
    }

    function M(a, b) {
        return a.classList.contains(b)
    }
    "classList" in document.createElement("_") || (q = function(a, b, c) {
        2 == arguments.length || c ? M(a, b) || (a.className += (0 < a.className.length ? " " : "") + b) : M(a, b) && (a.className = a.className.replace(new RegExp("(\\s|^)" + b + "(\\s|$)"), " "))
    }, M = function(a, b) {
        return !!a.className.match(new RegExp("(\\s|^)" + b + "(\\s|$)"))
    });

    function Ra(a) {
        q(a, "nav0open", !M(a, "nav0open"))
    }

    function ta() {
        return -1 != (window.navigator.userAgent || "").indexOf("IEMobile/") ? 2 : Math.max(window.devicePixelRatio || 1, 1)
    }

    function R(a, b, c) {
        a = a.getBoundingClientRect();
        return {
            x: b - a.left,
            y: c - a.top
        }
    }

    function Qd(a, b) {
        this.b = h(a, "div", b);
        this.a = 0
    }
    Qd.prototype.show = function(a) {
        if (this.a != a) {
            for (var b = this.b.children, c = 0, d = b.length; c < d; c++) "undefined" !== typeof b[c]["data-tab"] && u(b[c], {
                display: b[c]["data-tab"] == a ? "block" : "none"
            });
            this.a = a
        }
    };
    Qd.prototype.add = function(a, b, c) {
        b = c ? h(this.b, "div", b, c) : h(this.b, "div", b);
        b["data-tab"] = a;
        return b
    };

    function mc(a, b) {
        var c = a.indexOf("%s");
        return -1 != c ? a.substring(0, c) + b + a.substring(c + 2) : a
    }

    function nc(a) {
        a.innerHTML = a.innerHTML.replace(/\uD83C\uDFF4(?:\uDB40\uDC67\uDB40\uDC62(?:\uDB40\uDC65\uDB40\uDC6E\uDB40\uDC67|\uDB40\uDC77\uDB40\uDC6C\uDB40\uDC73|\uDB40\uDC73\uDB40\uDC63\uDB40\uDC74)\uDB40\uDC7F|\u200D\u2620\uFE0F)|\uD83D\uDC69\u200D\uD83D\uDC69\u200D(?:\uD83D\uDC66\u200D\uD83D\uDC66|\uD83D\uDC67\u200D(?:\uD83D[\uDC66\uDC67]))|\uD83D\uDC68(?:\u200D(?:\u2764\uFE0F\u200D(?:\uD83D\uDC8B\u200D)?\uD83D\uDC68|(?:\uD83D[\uDC68\uDC69])\u200D(?:\uD83D\uDC66\u200D\uD83D\uDC66|\uD83D\uDC67\u200D(?:\uD83D[\uDC66\uDC67]))|\uD83D\uDC66\u200D\uD83D\uDC66|\uD83D\uDC67\u200D(?:\uD83D[\uDC66\uDC67])|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E[\uDDB0-\uDDB3])|(?:\uD83C[\uDFFB-\uDFFF])\u200D(?:\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E[\uDDB0-\uDDB3]))|\uD83D\uDC69\u200D(?:\u2764\uFE0F\u200D(?:\uD83D\uDC8B\u200D(?:\uD83D[\uDC68\uDC69])|\uD83D[\uDC68\uDC69])|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E[\uDDB0-\uDDB3])|\uD83D\uDC69\u200D\uD83D\uDC66\u200D\uD83D\uDC66|(?:\uD83D\uDC41\uFE0F\u200D\uD83D\uDDE8|\uD83D\uDC69(?:\uD83C[\uDFFB-\uDFFF])\u200D[\u2695\u2696\u2708]|\uD83D\uDC68(?:(?:\uD83C[\uDFFB-\uDFFF])\u200D[\u2695\u2696\u2708]|\u200D[\u2695\u2696\u2708])|(?:(?:\u26F9|\uD83C[\uDFCB\uDFCC]|\uD83D\uDD75)\uFE0F|\uD83D\uDC6F|\uD83E[\uDD3C\uDDDE\uDDDF])\u200D[\u2640\u2642]|(?:\u26F9|\uD83C[\uDFCB\uDFCC]|\uD83D\uDD75)(?:\uD83C[\uDFFB-\uDFFF])\u200D[\u2640\u2642]|(?:\uD83C[\uDFC3\uDFC4\uDFCA]|\uD83D[\uDC6E\uDC71\uDC73\uDC77\uDC81\uDC82\uDC86\uDC87\uDE45-\uDE47\uDE4B\uDE4D\uDE4E\uDEA3\uDEB4-\uDEB6]|\uD83E[\uDD26\uDD37-\uDD39\uDD3D\uDD3E\uDDB8\uDDB9\uDDD6-\uDDDD])(?:(?:\uD83C[\uDFFB-\uDFFF])\u200D[\u2640\u2642]|\u200D[\u2640\u2642])|\uD83D\uDC69\u200D[\u2695\u2696\u2708])\uFE0F|\uD83D\uDC69\u200D\uD83D\uDC67\u200D(?:\uD83D[\uDC66\uDC67])|\uD83D\uDC69\u200D\uD83D\uDC69\u200D(?:\uD83D[\uDC66\uDC67])|\uD83D\uDC68(?:\u200D(?:(?:\uD83D[\uDC68\uDC69])\u200D(?:\uD83D[\uDC66\uDC67])|\uD83D[\uDC66\uDC67])|\uD83C[\uDFFB-\uDFFF])|\uD83C\uDFF3\uFE0F\u200D\uD83C\uDF08|\uD83D\uDC69\u200D\uD83D\uDC67|\uD83D\uDC69(?:\uD83C[\uDFFB-\uDFFF])\u200D(?:\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E[\uDDB0-\uDDB3])|\uD83D\uDC69\u200D\uD83D\uDC66|\uD83C\uDDF6\uD83C\uDDE6|\uD83C\uDDFD\uD83C\uDDF0|\uD83C\uDDF4\uD83C\uDDF2|\uD83D\uDC69(?:\uD83C[\uDFFB-\uDFFF])|\uD83C\uDDED(?:\uD83C[\uDDF0\uDDF2\uDDF3\uDDF7\uDDF9\uDDFA])|\uD83C\uDDEC(?:\uD83C[\uDDE6\uDDE7\uDDE9-\uDDEE\uDDF1-\uDDF3\uDDF5-\uDDFA\uDDFC\uDDFE])|\uD83C\uDDEA(?:\uD83C[\uDDE6\uDDE8\uDDEA\uDDEC\uDDED\uDDF7-\uDDFA])|\uD83C\uDDE8(?:\uD83C[\uDDE6\uDDE8\uDDE9\uDDEB-\uDDEE\uDDF0-\uDDF5\uDDF7\uDDFA-\uDDFF])|\uD83C\uDDF2(?:\uD83C[\uDDE6\uDDE8-\uDDED\uDDF0-\uDDFF])|\uD83C\uDDF3(?:\uD83C[\uDDE6\uDDE8\uDDEA-\uDDEC\uDDEE\uDDF1\uDDF4\uDDF5\uDDF7\uDDFA\uDDFF])|\uD83C\uDDFC(?:\uD83C[\uDDEB\uDDF8])|\uD83C\uDDFA(?:\uD83C[\uDDE6\uDDEC\uDDF2\uDDF3\uDDF8\uDDFE\uDDFF])|\uD83C\uDDF0(?:\uD83C[\uDDEA\uDDEC-\uDDEE\uDDF2\uDDF3\uDDF5\uDDF7\uDDFC\uDDFE\uDDFF])|\uD83C\uDDEF(?:\uD83C[\uDDEA\uDDF2\uDDF4\uDDF5])|\uD83C\uDDF8(?:\uD83C[\uDDE6-\uDDEA\uDDEC-\uDDF4\uDDF7-\uDDF9\uDDFB\uDDFD-\uDDFF])|\uD83C\uDDEE(?:\uD83C[\uDDE8-\uDDEA\uDDF1-\uDDF4\uDDF6-\uDDF9])|\uD83C\uDDFF(?:\uD83C[\uDDE6\uDDF2\uDDFC])|\uD83C\uDDEB(?:\uD83C[\uDDEE-\uDDF0\uDDF2\uDDF4\uDDF7])|\uD83C\uDDF5(?:\uD83C[\uDDE6\uDDEA-\uDDED\uDDF0-\uDDF3\uDDF7-\uDDF9\uDDFC\uDDFE])|\uD83C\uDDE9(?:\uD83C[\uDDEA\uDDEC\uDDEF\uDDF0\uDDF2\uDDF4\uDDFF])|\uD83C\uDDF9(?:\uD83C[\uDDE6\uDDE8\uDDE9\uDDEB-\uDDED\uDDEF-\uDDF4\uDDF7\uDDF9\uDDFB\uDDFC\uDDFF])|\uD83C\uDDE7(?:\uD83C[\uDDE6\uDDE7\uDDE9-\uDDEF\uDDF1-\uDDF4\uDDF6-\uDDF9\uDDFB\uDDFC\uDDFE\uDDFF])|[#\*0-9]\uFE0F\u20E3|\uD83C\uDDF1(?:\uD83C[\uDDE6-\uDDE8\uDDEE\uDDF0\uDDF7-\uDDFB\uDDFE])|\uD83C\uDDE6(?:\uD83C[\uDDE8-\uDDEC\uDDEE\uDDF1\uDDF2\uDDF4\uDDF6-\uDDFA\uDDFC\uDDFD\uDDFF])|\uD83C\uDDF7(?:\uD83C[\uDDEA\uDDF4\uDDF8\uDDFA\uDDFC])|\uD83C\uDDFB(?:\uD83C[\uDDE6\uDDE8\uDDEA\uDDEC\uDDEE\uDDF3\uDDFA])|\uD83C\uDDFE(?:\uD83C[\uDDEA\uDDF9])|(?:\uD83C[\uDFC3\uDFC4\uDFCA]|\uD83D[\uDC6E\uDC71\uDC73\uDC77\uDC81\uDC82\uDC86\uDC87\uDE45-\uDE47\uDE4B\uDE4D\uDE4E\uDEA3\uDEB4-\uDEB6]|\uD83E[\uDD26\uDD37-\uDD39\uDD3D\uDD3E\uDDB8\uDDB9\uDDD6-\uDDDD])(?:\uD83C[\uDFFB-\uDFFF])|(?:\u26F9|\uD83C[\uDFCB\uDFCC]|\uD83D\uDD75)(?:\uD83C[\uDFFB-\uDFFF])|(?:[\u261D\u270A-\u270D]|\uD83C[\uDF85\uDFC2\uDFC7]|\uD83D[\uDC42\uDC43\uDC46-\uDC50\uDC66\uDC67\uDC70\uDC72\uDC74-\uDC76\uDC78\uDC7C\uDC83\uDC85\uDCAA\uDD74\uDD7A\uDD90\uDD95\uDD96\uDE4C\uDE4F\uDEC0\uDECC]|\uD83E[\uDD18-\uDD1C\uDD1E\uDD1F\uDD30-\uDD36\uDDB5\uDDB6\uDDD1-\uDDD5])(?:\uD83C[\uDFFB-\uDFFF])|(?:[\u231A\u231B\u23E9-\u23EC\u23F0\u23F3\u25FD\u25FE\u2614\u2615\u2648-\u2653\u267F\u2693\u26A1\u26AA\u26AB\u26BD\u26BE\u26C4\u26C5\u26CE\u26D4\u26EA\u26F2\u26F3\u26F5\u26FA\u26FD\u2705\u270A\u270B\u2728\u274C\u274E\u2753-\u2755\u2757\u2795-\u2797\u27B0\u27BF\u2B1B\u2B1C\u2B50\u2B55]|\uD83C[\uDC04\uDCCF\uDD8E\uDD91-\uDD9A\uDDE6-\uDDFF\uDE01\uDE1A\uDE2F\uDE32-\uDE36\uDE38-\uDE3A\uDE50\uDE51\uDF00-\uDF20\uDF2D-\uDF35\uDF37-\uDF7C\uDF7E-\uDF93\uDFA0-\uDFCA\uDFCF-\uDFD3\uDFE0-\uDFF0\uDFF4\uDFF8-\uDFFF]|\uD83D[\uDC00-\uDC3E\uDC40\uDC42-\uDCFC\uDCFF-\uDD3D\uDD4B-\uDD4E\uDD50-\uDD67\uDD7A\uDD95\uDD96\uDDA4\uDDFB-\uDE4F\uDE80-\uDEC5\uDECC\uDED0-\uDED2\uDEEB\uDEEC\uDEF4-\uDEF9]|\uD83E[\uDD10-\uDD3A\uDD3C-\uDD3E\uDD40-\uDD45\uDD47-\uDD70\uDD73-\uDD76\uDD7A\uDD7C-\uDDA2\uDDB0-\uDDB9\uDDC0-\uDDC2\uDDD0-\uDDFF])|(?:[#\*0-9\xA9\xAE\u203C\u2049\u2122\u2139\u2194-\u2199\u21A9\u21AA\u231A\u231B\u2328\u23CF\u23E9-\u23F3\u23F8-\u23FA\u24C2\u25AA\u25AB\u25B6\u25C0\u25FB-\u25FE\u2600-\u2604\u260E\u2611\u2614\u2615\u2618\u261D\u2620\u2622\u2623\u2626\u262A\u262E\u262F\u2638-\u263A\u2640\u2642\u2648-\u2653\u265F\u2660\u2663\u2665\u2666\u2668\u267B\u267E\u267F\u2692-\u2697\u2699\u269B\u269C\u26A0\u26A1\u26AA\u26AB\u26B0\u26B1\u26BD\u26BE\u26C4\u26C5\u26C8\u26CE\u26CF\u26D1\u26D3\u26D4\u26E9\u26EA\u26F0-\u26F5\u26F7-\u26FA\u26FD\u2702\u2705\u2708-\u270D\u270F\u2712\u2714\u2716\u271D\u2721\u2728\u2733\u2734\u2744\u2747\u274C\u274E\u2753-\u2755\u2757\u2763\u2764\u2795-\u2797\u27A1\u27B0\u27BF\u2934\u2935\u2B05-\u2B07\u2B1B\u2B1C\u2B50\u2B55\u3030\u303D\u3297\u3299]|\uD83C[\uDC04\uDCCF\uDD70\uDD71\uDD7E\uDD7F\uDD8E\uDD91-\uDD9A\uDDE6-\uDDFF\uDE01\uDE02\uDE1A\uDE2F\uDE32-\uDE3A\uDE50\uDE51\uDF00-\uDF21\uDF24-\uDF93\uDF96\uDF97\uDF99-\uDF9B\uDF9E-\uDFF0\uDFF3-\uDFF5\uDFF7-\uDFFF]|\uD83D[\uDC00-\uDCFD\uDCFF-\uDD3D\uDD49-\uDD4E\uDD50-\uDD67\uDD6F\uDD70\uDD73-\uDD7A\uDD87\uDD8A-\uDD8D\uDD90\uDD95\uDD96\uDDA4\uDDA5\uDDA8\uDDB1\uDDB2\uDDBC\uDDC2-\uDDC4\uDDD1-\uDDD3\uDDDC-\uDDDE\uDDE1\uDDE3\uDDE8\uDDEF\uDDF3\uDDFA-\uDE4F\uDE80-\uDEC5\uDECB-\uDED2\uDEE0-\uDEE5\uDEE9\uDEEB\uDEEC\uDEF0\uDEF3-\uDEF9]|\uD83E[\uDD10-\uDD3A\uDD3C-\uDD3E\uDD40-\uDD45\uDD47-\uDD70\uDD73-\uDD76\uDD7A\uDD7C-\uDDA2\uDDB0-\uDDB9\uDDC0-\uDDC2\uDDD0-\uDDFF])\uFE0F|(?:[\u261D\u26F9\u270A-\u270D]|\uD83C[\uDF85\uDFC2-\uDFC4\uDFC7\uDFCA-\uDFCC]|\uD83D[\uDC42\uDC43\uDC46-\uDC50\uDC66-\uDC69\uDC6E\uDC70-\uDC78\uDC7C\uDC81-\uDC83\uDC85-\uDC87\uDCAA\uDD74\uDD75\uDD7A\uDD90\uDD95\uDD96\uDE45-\uDE47\uDE4B-\uDE4F\uDEA3\uDEB4-\uDEB6\uDEC0\uDECC]|\uD83E[\uDD18-\uDD1C\uDD1E\uDD1F\uDD26\uDD30-\uDD39\uDD3D\uDD3E\uDDB5\uDDB6\uDDB8\uDDB9\uDDD1-\uDDDD])/g, cd)
    }

    function fa(a) {
        document.domain && new Xe("/misc/k2rep.php", "fb=" + a)
    }

    function Ye(a) {
        var b = "";
        Object.keys(a || {}).forEach(function(c) {
            b += (b ? "&" : "") + encodeURIComponent(c) + "=" + encodeURIComponent(a[c])
        });
        return b
    }

    function Rd(a, b) {
        this.P = a;
        this.V = "undefined" == typeof b ? 4 : b;
        this.I = !1;
        this.j = this.v = this.A = 0;
        this.a = -1;
        this.B = this.f = this.J = 0;
        this.N = -1;
        this.L = this.G = 0
    }
    Rd.prototype.start = function() {
        var a = this;
        this.I = !0;
        this.A = setInterval(function() {
            return ye(a)
        }, Math.floor(1E3 / this.V))
    };

    function ye(a) {
        var b = Date.now();
        0 < a.v && b > a.v && (a.v = 0);
        if (-1 != a.a) {
            var c = b - a.j;
            b = 0;
            if (0 < a.f) {
                var d = a.f * Math.floor(200);
                c < d ? (b = Math.ceil((d - c) / 1E3), c = 0) : c -= d
            }
            c = a.J * Math.floor(200) + c * (0 < a.B ? 1 : -1);
            c = 0 < a.B ? Math.floor(c / 1E3) : Math.ceil(c / 1E3);
            if (a.a != a.N || c != a.G || b != a.L) {
                d = a.P;
                var f = a.a,
                    g = b;
                null != d.Bb ? (d.Bb = 0 > c ? "(?)" : Math.floor(c / 60) + ":" + Math.floor(c % 60 / 10) % 10 + c % 60 % 10, ve(d)) : (Id(d.T, f, c, g), qe(d));
                a.G = c;
                a.L = b;
                a.N = a.a
            }
        }
    }
    window.k2ver = 223;

    function pc(a) {
        var b = this;
        this.app = a;
        this.Oa = this.app.g("bl_buds");
        this.b = h(this.app.A, "div", {
            className: "stview usno",
            style: {
                display: "none"
            }
        });
        this.a = [];
        this.f = h(this.b, "div", {
            className: "clst btifp"
        });
        var c;
        h(this.b, "div", {
            className: "caddbox dcpd"
        }, [h("form", {
            onsubmit: function() {
                var a = c.value.trim();
                c.value = "";
                a && b.app.Y("/buddy " + a);
                c.blur();
                return !1
            }
        }, [h("p", {}, c = h("input", {
            className: "aid",
            name: "x",
            autocomplete: "off"
        })), h("p", {}, h("input", {
            type: "submit",
            className: "minw",
            value: this.app.g("bl_ad")
        }))])]);
        Xb(this, this.app.f)
    }

    function Ub(a, b) {
        a.tab = 0;
        a.La = "";
        a.status ? (a.La = b, b = 0 < a.fb.length ? a.fb[0] : 0, "#" == b ? a.tab = a.fb.substring(1) : ":" == b && (a.La += " (" + a.fb.substring(1) + ")")) : a.La = a.fb.substring(0, 10)
    }

    function Ze(a, b) {
        a.j || (a.j = z(a.app, "-"));
        var c;
        F(a.j.Ca, [a.app.J ? null : h("div", [h("p", h("a", {
            className: "lbut minwd",
            target: "_blank",
            href: mc(I(a.app, "stat"), encodeURIComponent(b)),
            onclick: function() {
                v(a.app)
            }
        }, a.app.g("ui_stats") + " >")), h("div", {
            className: "dtline"
        })]), h("p", [c = h("input", {
            type: "checkbox",
            checked: !0
        }), a.app.g("bl_buds")]), h("p", h("button", {
            className: "minw",
            onclick: function() {
                v(a.app);
                c.checked || a.app.Y("/unbuddy " + b)
            }
        }, a.app.g("bl_ok")))]);
        E(a.app, a.j, b)
    }

    function $e(a, b, c) {
        return zd(a.f, h("a", {
            className: "awrap dcpd" + (b.status ? " st1" : ""),
            onclick: function() {
                Ze(a, b.name);
                return !1
            }
        }, h("div", {
            className: "maxw"
        }, [h("div", {
            className: "chtbl",
            onclick: function(c) {
                lb(a.app, b.name);
                c.stopPropagation();
                return !1
            }
        }, h("div", {
            className: "spbb"
        })), h("div", {
            className: "uname"
        }, [h("div", {
            className: "sta"
        }), b.name]), h("div", {
            className: "infbl",
            style: {
                verticalAlign: "top"
            }
        }, [h("span", {
            className: "inftx"
        }, b.La), b.tab ? h("button", {
            className: "butbl butlh",
            onclick: function(c) {
                b.tab &&
                ab(a.app, b.tab);
                c.stopPropagation();
                return !1
            }
        }, "#" + b.tab) : null])])), c)
    }

    function af(a, b) {
        for (var c = a.a.length, d = b.status ? 1 : 0; 0 < c;) {
            var f = a.a[c - 1],
                g = f.status ? 1 : 0,
                k = b.name < f.name ? -1 : b.name > f.name ? 1 : 0;
            if (0 <= (1 == d ? 0 == g ? -1 : k : 1 == g ? 1 : b.La < f.La ? 1 : b.La > f.La ? -1 : k)) break;
            c--
        }
        d = a.a[c];
        a.a.splice(c, 0, b);
        b.C && a.f.insertBefore(b.C, d ? d.C : null)
    }

    function Yb(a, b) {
        b.C = $e(a, b);
        af(a, b)
    }

    function Zb(a, b) {
        var c = a.a.indexOf(b); - 1 != c && (b.C && (a.f.removeChild(b.C), b.C = null), a.a.splice(c, 1))
    }

    function Vb(a, b) {
        var c = a.a.indexOf(b); - 1 != c && (b.C = $e(a, b, b.C), a.a.splice(c, 1), af(a, b))
    }

    function Xb(a, b) {
        a.a.length = 0;
        Object.keys(b).forEach(function(c) {
            c = b[c];
            c.C = $e(a, c);
            af(a, c)
        }, a)
    }

    function rc(a) {
        function b(a) {
            return [h("b", a.Od + (0 == a.Ia ? "" : " (" + a.g("t_gsmp") + ")")), a.o.vb ? null : h("div", {
                className: "mlo r" + La(a, a.Ha)
            }), a.o.vb ? null : h("span", {
                className: "snum"
            }, J(a, a.Ha))]
        }
        this.app = a;
        this.Oa = this.app.g("bl_mr");
        this.b = h(a.A, "div", {
            className: "stvxpad vnarrow",
            style: {
                display: "none"
            }
        });
        var c = this;
        h(this.b, "div", {
            className: "btifp"
        });
        0 == this.app.Ia && h(this.b, [h("p", [h("button", {
            className: "minwd",
            onclick: function() {
                L(c.app, c.app.qf)
            }
        }, this.app.g("bl_prefs")), " ", h("button", {
            className: "minwd",
            onclick: function() {
                L(c.app, c.app.rf)
            }
        }, this.app.g("t_prof"))]), this.app.S ? null : h("p", {
            style: {
                marginTop: "-.5em"
            }
        }, [h("button", {
            className: "minwd",
            onclick: function() {
                wa(c.app)
            }
        }, this.app.g("t_lout"))]), h("hr")]);
        var d = h(this.b, "p", b(this.app));
        w(this.app, "urank", function() {
            F(d, b(c.app))
        });
        this.app.J && h(this.b, [h("hr"), h("p", h("button", {
            className: "minwd",
            onclick: function() {
                L(c.app, c.app.lf)
            }
        }, this.app.g("t_fb")))])
    }

    function sc(a) {
        this.app = a;
        this.Oa = this.app.g("t_fb");
        this.b = h(a.A, "div", {
            className: "stvxpad vnarrow",
            style: {
                display: "none"
            }
        });
        var b = this,
            c;
        h(this.b, [h("p", {
            className: "fb"
        }, this.app.g("t_sf")), c = h("textarea", {
            className: "bsbb",
            rows: 6,
            style: {
                width: "100%"
            }
        }), h("p", h("button", {
            className: "minw",
            onclick: function() {
                var a = b.app;
                L(a, a.V);
                a = c.value.trim();
                c.value = "";
                new Ve({
                    url: I(b.app, "feedback"),
                    data: "fb=" + a + "\n\n" + window.location.href + "\n" + screen.width + "x" + screen.height + "px",
                    od: !0
                });
                return !1
            }
        }, this.app.g("bl_ok")))])
    }

    function qc(a) {
        var b = this;
        this.app = a;
        this.Oa = this.app.g("bl_cs");
        this.f = this.v = null;
        this.a = {};
        this.j = [];
        this.b = h(this.app.A, "div", {
            className: "stview",
            style: {
                display: "none"
            }
        });
        this.I = h(this.b, "div", {
            className: "imvfrm"
        });
        this.G = h(this.b, "div", {
            className: "imvlst"
        });
        this.A = h(this.app.I, "div", {
            className: "fb fl",
            style: {
                display: "none",
                color: "#fff"
            }
        }, [h("div", {
            className: "sta"
        }), this.J = h("div", {
            className: "ib"
        })]);
        this.B = h(this.G, "div", {
            className: "iml btifp"
        });
        h(this.G, "p", {
            className: "dcpd"
        }, this.v = h("select", {
            style: {
                margin: "1em 0 .25em",
                minWidth: "10em"
            },
            onchange: function(a) {
                a = a.target;
                0 < a.selectedIndex && lb(b.app, a.options[a.selectedIndex].text);
                b.selectedIndex = 0
            }
        }));
        qb(this);
        a = this.app.L;
        var c = 90,
            d;
        for (d in a)
            if (a.hasOwnProperty(d) && a[d].Qa && 0 < c) {
                var f = bf(this, a[d].Ma, !0);
                cf(this, f);
                c--
            } 0 < this.j.length && (this.app.send([dd, 1], this.j), this.j = [])
    }
    qc.prototype.gf = function() {
        return this.f ? this.A : null
    };

    function df(a, b) {
        var c = a.app;
        if (c.B && c.B == a.b && (c = a.f)) {
            var d = a.app.L["_" + c.Ma];
            d && d.Qa && (a.app.send([25, d.Qa], [c.Ma]), d.Qa = 0, d.Bc = 0, b && c == b || cf(a, c))
        }
    }

    function ef(a) {
        if (a.app.ra && !a.f && a.B.firstChild) {
            var b = a.B.firstChild,
                c;
            for (c in a.a)
                if (a.a.hasOwnProperty(c) && a.a[c].ga == b) {
                    mb(a, a.a[c].Ma);
                    break
                }
        }
    }

    function cf(a, b) {
        if (b) {
            var c = a.app.L["_" + b.Ma];
            if (!c) return;
            c.Qa && F(b.Dd, [c.Bc]);
            u(b.Dd, {
                visibility: c.Qa ? "inherit" : "hidden"
            })
        }
        kb(a.app)
    }

    function ff(a, b, c) {
        var d = a.app.sa["_" + b];
        d && d.se && c.u.append("[" + a.app.g("chr") + "]", !0);
        (a = a.app.L["_" + b]) && c.u.append(a.zc)
    }

    function bf(a, b, c) {
        function d() {
            var c = this.selectedIndex;
            c && (1 == c ? a.app.send([24], [b]) : 2 == c && (a.app.Y("/ignore " + b), gf(a, b)), this.selectedIndex = 0)
        }
        var f = "_" + b;
        if (a.a.hasOwnProperty(f)) return a.a[f];
        a.a[f] = f = {
            b: null,
            u: null,
            Ma: b,
            ff: !1,
            ga: null,
            Dd: null
        };
        f.b = h(a.I, "div", {
            style: {
                display: "none"
            }
        });
        var g = ["...", a.app.g("chd"), a.app.g("ui_block") + " (" + b + ")"];
        f.u = new ad(a.app, f.b, {
            ue: function(c) {
                c.length > Ea && (c = c.substring(0, Ea) + "...");
                a.app.send([21], [b, c])
            },
            $e: function() {
                a.app.send([23], [b])
            },
            Jd: !0,
            cf: {
                className: "imfr"
            },
            re: {
                className: "imtx",
                style: {
                    minHeight: "5em"
                }
            },
            sf: {
                className: "imin"
            }
        });
        var k = h(f.u.gb.parentNode.parentNode, "div", {
            className: "imo1"
        });
        h(k, "select", {
            className: "drops",
            onchange: d
        }, g.map(function(a) {
            return h("option", a)
        }));
        k = h(f.b, "p", {
            className: "imo2"
        });
        h(k, "select", {
            className: "drops",
            onchange: d
        }, g.map(function(a) {
            return h("option", a)
        }));
        f.ga = h(a.B, "a", {
            className: "awrap dcpd",
            onclick: function() {
                lb(a.app, b);
                return !1
            }
        }, [h("div", {
                className: "clbt",
                onclick: function(c) {
                    gf(a, b);
                    c.stopPropagation();
                    return !1
                }
            },
            "X"), h("div", {
            className: "sta"
        }), h("div", {
            className: "ib"
        }, b), f.Dd = h("div", {
            className: "unrd",
            style: {
                visibility: "hidden"
            }
        }, "0")]);
        ff(a, b, f);
        c ? a.j.push(b) : a.app.send([dd, 1], [b]);
        return f
    }

    function gf(a, b) {
        var c = "_" + b;
        if (a.a.hasOwnProperty(c)) {
            var d = a.a[c];
            d && (d.ga.parentNode.removeChild(d.ga), d.b.parentNode.removeChild(d.b), delete a.a[c], a.app.send([dd, 0], [b]), a.f == d && mb(a, null))
        }
    }

    function mb(a, b) {
        var c = null,
            d = null;
        b && (c = a.a.hasOwnProperty(d = "_" + b) ? a.a[d] : bf(a, b));
        c != a.f && (a.f && (u(a.f.b, {
            display: "none"
        }), q(a.f.ga, "slctd", !1)), (a.f = c) ? (q(a.b, "imact", !0), F(a.J, [c.Ma]), q(a.A, "st1", c.ff), u(c.b, {
            display: "block"
        }), q(c.ga, "slctd"), c.u.Ea(), a.app.S && c.u.gb.focus(), df(a)) : q(a.b, "imact", !1))
    }

    function qb(a) {
        var b = a.app.nb.slice(0);
        b.unshift("-- " + a.app.g("bl_cs") + " --");
        a.v.options.length = 0;
        h(a.v, b.map(function(a) {
            return h("option", a)
        }));
        b = a.app.le;
        for (var c, d = 0; d < b.length && 90 > d; d++)(c = a.a["_" + b[d]]) ? ff(a, b[d], c) : bf(a, b[d], !0);
        0 < a.j.length && (a.app.send([dd, 1], a.j), a.j = []);
        ef(a)
    }

    function xb(a, b, c) {
        if (b = a.a["_" + b]) b.ff = c, q(b.ga, "st1", c), b == a.f && q(a.A, "st1", c)
    }

    function vb(a, b, c, d, f, g) {
        var k = a.a["_" + b];
        if (c == tb || c == sb) k ? k.u.append(d) : f && (k = bf(a, b)), f && (df(a, k) || cf(a, k)), g && (a = a.v, bd(a, h("option", b), 1 < a.length ? a.options[1] : null));
        else if (-1 == c) k && k.u.append(d, !0);
        else if (c == ub)
            for (k && k.u.reset(), a = a.v.options, c = a.length - 1; 0 <= c; c--)
                if (a[c].text == b) {
                    a.remove(c);
                    break
                }
    }
    qc.prototype.onshow = function() {
        this.f && this.f.u.Ea();
        df(this);
        var a = this.f;
        ef(this);
        a && this.app.S && a.u.gb.focus()
    };
    qc.prototype.Pb = function() {
        var a = Na();
        a && !/^[a-z0-9~]+$/i.test(a) && (a = null);
        this.app.ra || mb(this, a)
    };

    function tc(a) {
        this.app = a;
        this.Oa = this.app.g("bl_prefs");
        this.b = h(a.A, "div", {
            className: "stvxpad vnarrow",
            style: {
                display: "none"
            }
        });
        var b = this;
        h(this.b, [h("p", [this.f = h("input", {
            type: "checkbox"
        }), this.app.g("p_ignprv")]), h("p", [this.a = h("input", {
            type: "checkbox"
        }), this.app.g("p_prvbud")]), h("p", [this.j = h("input", {
            type: "checkbox"
        }), this.app.g("p_igninv")]), h("p", h("button", {
            className: "minw",
            onclick: function() {
                var a = b.app,
                    d = b.a.checked;
                a.pc = b.f.checked;
                a.nc = d;
                a = b.app;
                a.sc = b.j.checked;
                Ga(a);
                L(b.app, b.app.V);
                return !1
            }
        }, this.app.g("bl_ok")))])
    }
    tc.prototype.onshow = function() {
        this.f.checked = this.app.pc;
        this.a.checked = this.app.nc;
        this.j.checked = this.app.sc
    };

    function Oa(a) {
        this.app = a;
        this.Vd = !0;
        this.Ud = !1;
        this.b = h(a.A, "div", {
            className: "astat bsbb",
            style: {
                display: "none"
            },
            ontouchmove: function() {
                return !1
            }
        })
    }

    function uc(a, b) {
        var c = this;
        this.app = a;
        this.o = b;
        this.Oa = b.Cb || "-";
        this.f = 0;
        this.A = 1;
        this.v = null;
        this.j = 0;
        this.b = h(a.A, "div", {
            className: "stview",
            style: {
                display: "none"
            }
        });
        this.B = h(this.b, "p", {
            className: "tac"
        }, h("div", {
            className: "loader"
        }));
        this.ga = h(this.b, "div", {
            className: "turs",
            style: {
                display: "none"
            }
        });
        window[this.o.pb] = function(a, b) {
            return c.tb(a, b)
        }
    }

    function hf(a, b) {
        u(a.ga, {
            display: b ? "block" : "none"
        });
        u(a.B, {
            display: "none"
        });
        a.j = 0;
        if (!b) var c = a.j = setTimeout(function() {
            a.j == c && (a.j = 0, u(a.B, {
                display: "block"
            }))
        }, 500)
    }

    function jf(a) {
        var b = a.v = new Xe(a.o.url + (2 == a.f ? "&sk=2&page=" + a.A : ""), Ye(a.o.Cd ? {
            jsget: 1,
            ksession: sa(a.app)
        } : {
            jsget: 1
        }));
        b.handle = function(c, d) {
            return a.handle(b, c, d)
        }
    }
    uc.prototype.Pb = function(a) {
        var b = Na(),
            c = 1,
            d = 1,
            f;
        b && (f = b.split("/")) && "f" == f[0] && (c = 2, b = parseInt(f[1], 10), isNaN(b) || (d = b));
        if (a || c != this.f || d != this.A) 0 != this.f && hf(this, 0), this.f = c, this.A = d, jf(this)
    };

    function kf(a, b) {
        function c(a) {
            return b.tx && b.tx[a] || a
        }
        var d = b.tl;
        h(a.ga, "div", {
            className: "bwrap dcpd"
        }, [h("a", {
            href: "",
            onclick: function() {
                L(a.app, a.app.Wd);
                return !1
            }
        }, c("t_hupt")), " | ", h("a", {
            href: "",
            onclick: function() {
                L(a.app, a.app.Wd + "/f");
                return !1
            }
        }, c("t_hfit"))]);
        a.a = h(a.ga, "table", {
            className: "tulst",
            style: {
                width: "100%",
                borderCollapse: "collapse"
            }
        });
        for (var f in d) {
            var g = d[f];
            var k = a.a.insertRow(-1);
            if (1 == a.f) {
                var m = h(k.insertCell(-1), "a", {
                    className: "awrap dcpd hv"
                });
                k = h(m, "div", {
                    className: "maxw"
                });
                var l = h(k, "div", {
                    className: "bl1"
                });
                h(l, "div", {
                    className: "tid"
                }).innerHTML = '<b class="lc">' + g.id + "</b> (" + g.nop + ")";
                l = h(l, "div", {
                    className: "torg"
                });
                l.innerHTML = "" + g.onm + "";
                l = h(k, "div", {
                    className: "bl2"
                });
                k = h(l, "div", {
                    className: "tpar"
                });
                k.innerHTML = g.par;
                var n = h(l, "div", {
                    className: "tdtup"
                });
                n.innerHTML = g.dt;
                (function(b) {
                    m.onclick = function() {
                        a.app.Y("/join " + b);
                        return !1
                    }
                })(g.id)
            } else m = h(k.insertCell(-1), "div", {
                className: "awrap dcpd"
            }), k = h(m, "div", {
                className: "maxw"
            }), l = h(k, "div", {
                className: "bl1"
            }), n =
                h(l, "div", {
                    className: "tdtfin"
                }), n.innerHTML = g.dt, l = h(l, "div", {
                className: "torg"
            }), l.innerHTML = "" + g.onm + "", l = h(k, "div", {
                className: "bl2"
            }), k = h(l, "div", {
                className: "tpar"
            }), k.innerHTML = g.par, h(l, "a", {
                className: "tdt",
                href: g.resurl,
                target: "_blank"
            }, [c("t_trsl"), " (" + g.nop + ")"])
        }
        d = h(a.ga, "div", {
            className: "dcpd"
        });
        2 == a.f && b.page && h(d, "p", {}, h("a", {
            className: "fb",
            href: "",
            onclick: function() {
                L(a.app, a.app.Wd + "/f/" + (b.page + 1));
                return !1
            }
        }, c("t_next") + " >"));
        h(d, "p", {
            style: {
                marginTop: "2em"
            }
        }, h("button", {
            className: "minw",
            onclick: function() {
                L(a.app, a.app.mf)
            }
        }, c("t_tune")));
        if ((f = b.toft) && 0 < f.length) {
            g = h(d, "div", {
                style: {
                    marginTop: "1.5em"
                }
            });
            h(g, "p", c("t_oftt"));
            g = h(g, "p", {
                className: ""
            });
            for (var p in f) g.innerHTML += f[p] + "<br />"
        }
        b.tcur && h(d, "p", {
            marginTop: "1.5em"
        }, b.tcur)
    }
    uc.prototype.tb = function() {};
    uc.prototype.handle = function(a, b, c) {
        a == this.v && (this.v = null, b && (this.ga.innerHTML = c.html || "", kf(this, c), hf(this, 1)))
    };

    function O(a, b) {
        var c = this;
        this.app = a;
        this.o = b;
        this.Oa = b.Cb || "-";
        this.f = null;
        this.a = 0;
        this.b = h(a.A, "div", {
            className: "stvxpad vnarrow",
            style: {
                display: "none"
            }
        });
        this.v = h(this.b, "p", {
            className: "tac"
        }, h("div", {
            className: "loader"
        }));
        this.j = h(this.b, "div", {
            style: {
                display: "none"
            }
        });
        window[this.o.pb] = function(a, b) {
            return c.tb(a, b)
        }
    }

    function lf(a, b) {
        u(a.j, {
            display: b ? "block" : "none"
        });
        u(a.v, {
            display: "none"
        });
        a.a = 0;
        if (!b) var c = a.a = setTimeout(function() {
            a.a == c && (a.a = 0, u(a.v, {
                display: "block"
            }))
        }, 500)
    }
    O.prototype.onshow = function() {
        var a = this;
        lf(this, 0);
        var b = this.f = new Xe(this.o.url, Ye(this.o.Cd ? {
            jsget: 1,
            ksession: sa(this.app)
        } : {
            jsget: 1
        }));
        b.handle = function(c, d) {
            return a.handle(b, c, d)
        }
    };
    O.prototype.tb = function(a, b) {
        var c = this;
        if ("fsub" == a) {
            lf(this, 0);
            var d = this.f = new Xe(this.o.url, new FormData(b));
            d.handle = function(a, b) {
                return c.handle(d, a, b)
            }
        }
        this.o.tb && this.o.tb(a, b)
    };
    O.prototype.handle = function(a, b, c) {
        if (a == this.f && (this.f = null, b))
            if (c.ksession) {
                a = this.app;
                try {
                    window.localStorage.setItem("ksession", c.ksession)
                } catch (d) {}
                a.gc && (document.cookie = "kguest=1;path=/");
                ja(a, !0)
            } else c.done ? (c = this.app, c.B && c.B == this.b && (c = this.app, L(c, c.V))) : (this.j.innerHTML = c.html || "", c.script && h(this.j, "script", {
                type: "text/javascript",
                text: c.script
            }), lf(this, 1))
    };
    window.k2snd = {};
    window.k2snd["a.mp3"] = "data:audio/mpeg;base64,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";
    window.k2style = '.h1s{height:44px;box-sizing:border-box}.aleg .h1s{height:40px;box-sizing:border-box}.lh1s{line-height:44px}.aleg .lh1s{line-height:40px}.mb1s{margin-bottom:44px}.aleg .mb1s{margin-bottom:40px}.hdhei{height:46px}.hdbwd{width:54px}.k2base{background:#fff;color:#000;position:relative}.k2base{height:auto;min-height:100%}.k2base.asizing{margin:0 auto}.k2base{-webkit-tap-highlight-color:rgba(0,0,0,0)}.k2base.devios{cursor:pointer}.k2base{font:16px/1.4 arial,sans-serif}.k2base.aleg{font:14px/1.4 arial,sans-serif}.win .k2base.aleg{font:13px/1.5 verdana,sans-serif}.k2base{-webkit-text-size-adjust:100%}.k2base{-ms-touch-action:pan-y}.anav{display:none;position:fixed;top:0;left:0;right:0;height:46px;line-height:46px;text-align:center;background:#2258bb;overflow:hidden}.asizing .anav{position:absolute}.donav .anav{display:block}.aleg .anav{position:absolute;background:#9ce}.acon{position:absolute;top:0;bottom:0;left:0;right:0}.donav:not(.aleg) .acon{position:static;padding-top:46px}.aleg.donav .acon{top:46px;overflow-y:auto}.vm2 .aleg.donav.k2base{border-style:solid;border-color:#9ce;border-width:0 8px 0 8px}.vm2 .aleg.donav .acon{border-bottom:solid 44px #9ce}.stview{padding:0}.stvxpad{padding-left:40px;padding-right:40px}.vm0 .stvxpad,.vm1 .stvxpad{padding-left:15px;padding-right:15px}.vm2 .stview,.vm2 .stvxpad{padding:37px 40px}.stview,.stvxpad{border-bottom:solid 1px transparent}.vm2 .btifp{border-top:solid 1px #e4e4e4}.dclpd,.dcpd{padding-left:40px}.dcrpd,.dcpd{padding-right:40px}.vm0 .dclpd,.vm0 .dcpd{padding-left:15px}.vm0 .dcrpd,.vm0 .dcpd{padding-right:15px}.vnarrow{max-width:520px;margin:0 auto}.vm0 .nifvm0{display:none}.ib{display:inline-block}.fb{font-weight:bold}.drops{width:3em}.nowrel{white-space:nowrap;overflow:hidden;text-overflow:ellipsis}.wsnw{white-space:nowrap}hr{border:0;height:1px;background-color:#e4e4e4}.snum{font-size:14px}.aleg .snum{font-size:13px}.win .snum{font-size:12px}.f12{font-size:12px}.win .f12{font-size:11px}.fl{font-size:18px}.emo{font-size:150%;line-height:.9;vertical-align:-0.22em}.win .emo{vertical-align:-0.1em}a:link,a:visited,.lc{color:#25b;text-decoration:none}input:not([type]),input[type=text],input[type=password],textarea{font:inherit;line-height:1.4;color:inherit;margin:0;border:solid 1px rgba(0,0,0,0.3);border-radius:0;padding:6px 2px;outline:none;-webkit-appearance:none}.aleg input:not([type]),.aleg input[type=text],.aleg input[type=password],.aleg textarea{padding:4px 2px;line-height:1.4}button,input[type="submit"],a.lbut{font:inherit;line-height:1.4;background:none;color:inherit;margin:0;border:solid 1px rgba(0,0,0,0.4);border-radius:4px;padding:6px 12px;outline:none;display:inline-block}.devtch button:active{opacity:.5}button:not(.butsys)[disabled],input[type="submit"][disabled]{opacity:.6}.aleg button,.aleg input[type="submit"],.aleg a.lbut{padding:4px 10px}.aleg button:not([disabled]){cursor:pointer}.butsys{background:#f8f8f8;border-color:rgba(0,0,0,0.3)}.butsys[disabled]{color:#888}.butwb{background:#fff;color:#222;border-color:transparent}.butlh,.aleg .butlh{padding:1px 10px}.butbl{background:rgba(32,96,180,0.2);color:rgba(32,96,180,0.8);border-color:transparent}.ddcont{display:none}.ddbut:focus+.ddcont,.devios .ddbut:hover+.ddcont{display:block}.ddcont.ddopen{display:block}.ddcont:active{display:block}a.lbut{color:inherit;text-align:center}select{font:inherit;margin:0;height:1.9em;outline:none}input[type=file]{font:inherit;box-sizing:border-box;max-width:100%}input[type=checkbox],input[type=radio]{margin-left:0;vertical-align:middle}button::-moz-focus-inner{padding:0;border:none}.selcwr{position:relative;display:inline-block;line-height:normal}.selcbt,.aleg .selcbt{text-align:left;overflow:hidden;text-overflow:ellipsis;padding-right:20px;white-space:nowrap}.selcbt::before{content:"";width:0;height:0;display:inline-block;position:absolute;right:8px;top:44%;border-top:solid 5px #555;border-left:solid 4px transparent;border-right:solid 4px transparent}.selcsl{opacity:0;position:absolute;box-sizing:border-box;left:0;top:0;width:100%;height:100%}.noth{-webkit-tap-highlight-color:rgba(0,0,0,0)}.tama{touch-action:manipulation}.bs{box-shadow:0 0 1px 1px rgba(0,0,0,0.15);-webkit-box-shadow:0 0 1px 1px rgba(0,0,0,0.15)}.usno{-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none}.bsbb,.k2base{box-sizing:border-box;-webkit-box-sizing:border-box}.mlh{margin-left:.5em}.mlo{margin-left:1em}.mro{margin-right:1em}.mbsp{margin-bottom:.5em}.mbh{margin-bottom:.5em}.mtbq{margin:.25em 0}.mtoh{margin-top:1.5em}.minm{min-width:50px}.minw{min-width:75px}.minwd{min-width:140px;box-sizing:border-box}.tind{text-indent:-0.85em;margin-left:.85em}.win .aleg .tind{text-indent:-1.1em;margin-left:1.1em}.tac{text-align:center}.ttup{text-transform:uppercase}.dsp1,.aleg .dsp1{padding-left:10px;padding-right:10px}.fw{max-width:520px}.fw label{display:block;margin:3px 0}.fw input:not([type]),.fw input[type=text],.fw input[type=password]{box-sizing:border-box;width:100%;max-width:360px}.la{display:inline-block;-webkit-touch-callout:none;text-align:center}.spbb{position:relative;display:inline-block;width:20px;height:11px;border-radius:2px}.spbb:after{content:"";position:absolute;bottom:-5px;left:6px;border-width:5px 5px 0 0;border-style:solid}.aleg .spbb{position:relative;display:inline-block;width:18px;height:10px;border-radius:2px}.aleg .spbb:after{content:"";position:absolute;bottom:-4px;left:5px;border-width:4px 4px 0 0;border-style:solid}.spbb{background:rgba(32,96,180,0.4)}.spbb:after{border-color:rgba(32,96,180,0.4) transparent}.uicon{display:inline-block;position:relative;width:12px;height:3px;border-radius:2px 2px 0 0}.uicon:after{content:"";position:absolute;left:3px;width:6px;height:6px;margin-top:-7px;border-radius:6px}.uicon,.uicon:after{background:#aaa}button .cmenu{display:inline-block;width:5px;height:4px;border-top:4px solid #fff;border-bottom:12px double #fff;margin-top:4px}button.active .cmenu{border-color:#888}.sta{display:inline-block;width:11px;height:11px;border-radius:5px;background:#ddd;margin-right:8px}.aleg .sta{width:10px;height:10px}.st1 .sta{background:#2c2}.darr{display:inline-block;width:0;height:0;border-left:solid 5px transparent;border-right:solid 5px transparent;border-top:solid 6px #e8e8e8}.loader{display:inline-block;width:12px;height:6px;background-image:url(data:image/gif;base64,R0lGODlhDAACAIABAAAAAP///yH/C05FVFNDQVBFMi4wAwEAAAAh+QQBMgABACwAAAAADAACAAACBIyPqVcAIfkEATIAAQAsAAAAAAwAAgAAAgYEgqmmHQUAIfkEATIAAQAsAAAAAAwAAgAAAgaMDXB7yQUAIfkEATIAAQAsAAAAAAwAAgAAAgaMjwDIoQUAOw==)}#pread{margin:0;padding:10px 0 6px 40px;border-bottom:solid 1px #e4e4e4}.vm0 #pread,.vm1 #pread{padding-left:20px}.szpan{display:none;position:absolute;top:0;left:50%;margin-top:-12px;line-height:24px;width:160px;margin-left:-80px;text-align:center}.dosize .szpan{display:block}.ifdosize{display:none}.dosize .ifdosize{display:block}.szpan .bmax{height:13px;line-height:13px;width:32px;border:none;border-radius:0;padding:0 0 1px 0;background:#e4e4e4;color:#fff;margin:0 1px;cursor:pointer}table.br{-webkit-tap-highlight-color:transparent}table.br td{padding:4px 8px;border-bottom:solid 1px #eee;overflow:hidden}.astat{height:100%;text-align:center}.aleg .astat{background:#def}.astat>table{width:100%;height:100%}.aleg .astat>table{height:auto}.astat>table td{vertical-align:middle;padding:10px 0}.dtline{border-top:solid 1px rgba(0,0,0,0.2);margin:1em 0}.taplain{border:none;margin:0;outline:0}.ovysct{overflow-y:scroll;-webkit-overflow-scrolling:touch}.scln{margin:.25em 0}.scp1{min-width:6em}.bdtab{table-layout:fixed;border-collapse:collapse;background:#fff;text-align:center;width:280px;margin:0 auto;border-radius:3px;box-shadow:0 0 1px 1px rgba(0,0,0,0.1)}.bdtab{line-height:42px}.aleg .bdtab{line-height:40px}.bdmpart .bdp2{display:none}.bdmp2on .bdp1{display:none}.bdmp2on .bdp2{display:table-row}.bdtab td{border:solid 1px #e8e8e8;padding:0}.bdtab tr:first-child td,.bdtab tr.trlfst td{border-top:none}.bdtab tr:last-child td{border-bottom:none}.bdtab td:first-child{border-left:none}.bdtab td:last-child{border-right:none}.bdtab td button{width:100%;line-height:inherit;padding:0;border:none;border-radius:0;background:none;color:#222;outline:none}.bdtab td button:active{background:rgba(0,0,0,0.1)}.bdtab td button[disabled]{opacity:.5}.bdtab td button[disabled]:active{background:none}.bdtab tr.invm0{display:none}.vm0 .bdtab tr.invm0{display:table-row}.bdtab tr.invm0{display:none}.vm0 .bdtab tr.invm0{display:table-row}.vm1 .bdtab tr.trlfstnotvm0 td,.vm2 .bdtab tr.trlfstnotvm0 td{border-top:none}.clst .awrap{display:block;border-bottom:solid 1px #e4e4e4;line-height:44px;white-space:nowrap}.devtch .clst .awrap:active{background:rgba(0,0,0,0.15)}.aleg .clst .awrap{line-height:34px}.clst .awrap button{margin-left:.5em}.clst .maxw{max-width:420px}.clst .uname{display:inline-block;width:47%;font-weight:bold}.clst .infbl{display:inline-block}.clst .chtbl{display:inline-block;float:right;padding:0 16px;margin-right:-15px}.aleg .clst .uname{font-weight:normal}.aleg .clst .st1 .uname{font-weight:bold}.caddbox{margin:2.5em 0}.caddbox .aid{min-width:200px}.vm0 .clst .uname{width:auto}.vm0 .clst .inftx{display:none}.hvok .clst .awrap:hover{background:#eaecef;cursor:pointer}.ctcont{position:absolute;margin:0 auto;left:0;right:0}.ctcont button{padding:4px 10px}.cttal{text-align:left}.cttac{text-align:center}.ctcont{line-height:40px}.aleg .ctcont{line-height:36px}.ctst0{background:#fff;color:#000;width:300px}.ctst1{background:#333;color:#eee;width:300px}.ctst1 button{border-color:rgba(255,255,255,0.8);border-radius:2px;margin-right:.2em}.ctst0 .ctmsg+div,.ctst1 .ctmsg+div{margin-top:-6px;margin-bottom:6px}.ctcontcard{position:absolute;margin:0 auto;left:0;right:0;text-align:center}.ctcontcard button{border-color:#fff;color:#fff;border-radius:2px;padding:3px 10px}.ctcontlim4{white-space:nowrap}.ctcontlim4 button{min-width:45px;max-width:95px;white-space:nowrap;overflow:hidden}.ctnomsg .ctmsg{display:none}.ctstgs{background:#fff0b0}.ctstgs .ctpan{margin:0 1em}.ctstcol,.aleg .ctstcol{min-width:1.7em;margin-right:3px;padding:0;border-color:transparent}.vm2 .aleg .tblobby{height:100%}.tbvusers{display:none}.tbact .tbvtabs{display:none}.tbact .tbvusers{display:block}.vm2 .aleg .tbvtabs{display:block;float:left;width:71%}.vm2 .aleg .tbvusers{display:block;float:right;width:29%}.vm2 .aleg .tbvtabs{height:100%;overflow-y:scroll}.vm2 .aleg .tbvusers{height:100%;overflow-y:scroll}.newtab2{display:block;border-bottom:solid 1px #e4e4e4;padding-top:12px;padding-bottom:12px;white-space:nowrap}.tumode .newtab1 .vsel{width:5em}.vm2 .aleg .newtab2{display:none}.min85{min-width:85px}.tldeco{display:none}.vm2 .aleg .tldeco{display:block;height:16px;border-bottom:solid 1px #e4e4e4}.vm2 .aleg #pread~.tldeco{display:none}.alrt{padding-top:10px;padding-bottom:10px;border-bottom:solid 1px #e4e4e4;background:#fff0b0;color:rgba(0,0,0,0.9)}.tuinfo{display:none}.tumode .tuinfo{display:inline-block}.chpan{display:none;padding-top:10px;padding-bottom:10px;border-bottom:solid 1px #e4e4e4}.chmode .chpan{display:block}.chtop{color:#808080}.chsub{display:none;margin-top:10px}.chopen .chsub{display:block}.chgrlist{display:none;margin-right:15px}.chgrp.chopen .chgrlist{display:inline-block}.imvfrm{display:none}.imact .imvlst{display:none}.imact .imvfrm{display:block}.vm2 .imvfrm{display:block;float:left;width:60%}.vm2 .imvlst{display:block;float:right;width:35%;margin-left:5%}.iml .awrap{display:block;line-height:44px;border-bottom:solid 1px #e4e4e4;font-weight:bold;cursor:pointer}.devtch .iml .awrap:active{background:rgba(0,0,0,0.15)}.aleg .iml .awrap{line-height:34px;font-weight:normal}.iml .slctd{background:#e8e8e8}.aleg .iml .st1{font-weight:bold}.iml .clbt{float:right;font-weight:normal;color:#ccc;padding:0 15px;margin-right:-15px}.iml .unrd{display:inline-block;margin-left:8px;background:#e22;color:#fff;line-height:1.2;font-weight:bold;border-radius:14px;padding:0 12px 1px 12px}.imtx{padding:4px 15px}.vm2 .imtx{border:solid 1px #aaa;height:150px;overflow-y:scroll;-webkit-overflow-scrolling:touch}.imin{background:#f8f8f8;border-top:solid 1px #ddd;padding:8px 15px}.vm2 .imin{background:transparent;border-top:none;padding:8px 0}.imo1{display:table-cell;width:1%;padding-left:1em}.imo2{display:none}.vm2 .imo1{display:none}.vm2 .imo2{display:block;margin-top:.5em}.navttl{color:#fff;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;margin:0 80px}.navcont{width:100%;text-align:left;white-space:nowrap}.newtab1{display:inline-block;min-width:340px}.newtab1 .vsel{width:160px}.nav button{background:rgba(0,20,60,0.2);color:#fff;border-color:transparent;border-radius:0;margin-right:1px;min-width:5em}.nav button.bmain{min-width:6em}.nav button.btab{min-width:6em;margin-left:1.5em;background:#fc4;color:#fff;display:none}.nav button.alert{background:#e21;color:#fff}.nav button.active{background:#fff;color:#4c7a9c}.nav button .spbb{background:rgba(255,255,255,0.9)}.nav button .spbb:after{border-color:rgba(255,255,255,0.9) transparent}.nav button.alert .spbb{background:#fff}.nav button.alert .spbb:after{border-color:#fff transparent}.nav button.active .spbb{background:#4c7a9c}.nav button.active .spbb:after{border-color:#4c7a9c transparent}.navtabopen .nav button.btab{display:inline-block}.nav0{position:fixed;left:0;top:0;display:none}.asizing .nav0{position:absolute}.doddmenu .nav0{display:block}.nav0>.mbut{border:none;border-radius:0;padding:0;margin:0;color:#fff}.nav0>.mbut .micon{display:inline-block;width:20px;height:3px;border-top:3px solid #fff;border-bottom:9px double #fff;margin-top:3px}.nav0>.mbut.alert{background:rgba(255,0,0,0.4)}.thcol1 .mbut .micon{border-color:#999}.thcol2 .mbut .micon{border-color:#378a4a}.nav0open{background:#fff;bottom:0;border-right:solid 1px #ddd;overflow-y:auto}.nav0open>.mbut .micon{border-color:#777}.nav0open>.mbut.alert{background:#fff}.nav0 .mcont{display:none}.nav0open .mcont{display:block;min-width:250px;margin-top:-1px}.nav0 .mlst{border-top:solid 1px #eee}.nav0 .mlst button{font:inherit;display:block;width:100%;text-align:left;color:#000;border:none;border-radius:0;border-bottom:solid 1px #eee;padding:0 17px;line-height:44px}.nav0 .mlst button:active{background:rgba(0,0,0,0.15)}.nav0 .mlst button:xlast-child{border-bottom:solid 1px #ddd}.nav0 .mlst button.btab{display:none}.nav0 .mlst button.alert{color:#e21;font-weight:bold}.nav0 .mlst button.active{font-weight:bold}.nav0 .mlst button .spbb{background:#aaa}.nav0 .mlst button .spbb:after{border-color:#aaa transparent}.nav0 .mlst button.alert .spbb{background:#e21}.nav0 .mlst button.alert .spbb:after{border-color:#e21 transparent}.navtabopen .nav0 .mlst button.btab{display:block}.nav0 .msub{padding:.5em 15px}button.ubut{border-color:transparent}.tbact button.ubut{background:#e4e4e4}.r0,.r1,.r2,.r3,.r4,.r5,.rnone{display:inline-block;vertical-align:baseline;border-radius:2px;margin-right:6px}.r0,.r1,.r2,.r3,.r4,.r5,.rnone{width:10px;height:8px;border:solid 1px rgba(0,0,0,0.01)}.aleg .r0,.aleg .r1,.aleg .r2,.aleg .r3,.aleg .r4,.aleg .r5,.aleg .rnone{width:8px;height:7px}.rnone,.aleg .rnone{border-color:transparent}.r0{background:#d0d0d0}.r1{background:#00d0f8}.r2{background:#00bf00}.r3{background:#eaea10}.r4{background:#ff9910}.r5{background:#ee4242}.bcont{position:absolute;left:0;top:0;right:0;bottom:0}.bcont{-ms-touch-action:none}.bhead{display:none;height:46px}.gvhead .bhead{display:table}.gvhead .bcont{top:46px}.gvnohead{display:none}.gview:not(.gvhead) .gvnohead{display:block}.wcol,.wcolhd{color:#fff}.wcol button{border-color:#fff}.wcolhd button{padding:3px 12px;border-color:#fff;margin-right:4px}.wcol button.alert,.wcolhd button.alert{background:#e11}.gview{position:relative;width:100%;height:100%;overflow:hidden}.iosfix .gview{position:fixed}@media screen and (orientation:landscape) and (min-width:598px){.devmob.h100vh .gview{height:100vh;min-height:314px}}@media screen and (orientation:portrait) and (device-height:480px){.gview{min-height:416px}}.thead{display:none;position:absolute;top:0;left:0;right:0}.thead{color:#fff;border-bottom:solid 1px #fff}.thcol1 .thead{color:#999;border-bottom-color:#999}.thcol2 .thead{color:#378a4a;border-bottom-color:#378a4a}.gvhead .thead{display:block}.thnavcont{display:none;color:#fff}.thcol1 .thnavcont{color:#999}.thcol2 .thnavcont{color:#378a4a}.sbdrop .thnavcont{display:block}.xbut{background:transparent;border:none;color:inherit;position:absolute;top:0}.cmenubut{background:transparent;border:none;color:inherit;position:absolute;top:0}.thcol1 .cmenu{border-color:#999}.thcol2 .cmenu{border-color:#378a4a}.sbdropvis .cmenubut{background:#fff}.sbdropvis .cmenu{border-color:#888}.tsb{position:absolute;top:0;right:0;bottom:0;overflow-y:auto;display:none}.sbfixed .tsb{width:310px}.sbdrop .tsb{width:100%;top:46px}.sbdrop .tsb{display:block;visibility:hidden;z-index:98}.sbdropvis .tsb{visibility:inherit}.sbfixed .tsb{display:block}.sbfixed .bcont{margin-right:310px}.tsbinner{padding:5px 10px 10px}.sbfixed .tsbinner{min-height:100%}.sbclrd{background:#9ce;color:#123}.tsinbo{display:none}.sbfixed .tsinbo{display:table;visibility:hidden}.tstatact .tsinbo{visibility:inherit}.tstatinbohide .tsinbo{visibility:hidden}.tsinsb{display:none}.sbdrop .tsinsb{display:block;margin:4px 0 3px}.tstatstart .tsinsb .tstatlabl{display:none}.tstatstrl{display:none}.tstatstart .tstatstrl{display:block}.ttlcont{white-space:nowrap}.sbdrop .ttlcont{font-weight:bold;text-align:center;line-height:44px}.sbfixed .ttlcont{font-weight:bold;border-bottom:solid 1px rgba(0,0,0,0.25);line-height:46px}.ttlnav{display:none}.ttlnav button{min-width:42px;margin-left:2px}.sbfixed .ttlnav{display:block;float:right;font-weight:normal;text-align:right}.ttlnav button.alert{background:#e21;color:#fff;border-color:transparent}.sbfixed .tplcont{margin-top:15px}.sbfixed .trqcont{margin-top:20px;border-top:solid 1px rgba(0,0,0,0.25)}.trqcont .trqans{background:#234;color:#fff}.tctres{display:none}.sbfixed .tctres{display:block;margin-top:15px}.trk{display:none}.sbfixed .trk{display:block;margin-top:15px;border-top:solid 1px rgba(0,0,0,0.25)}.sbfixed .tcrdcont{margin-top:15px}.sbfixed .tcrdspecgs{margin-top:4px}.sbfixed .tcrdpan{height:160px}.sbdrop .tcrdpan{height:160px}.tsbchat{display:none}.sbfixed .tsbchat{display:block}.tinbcht{display:none}.sbdrop .tinbcht{display:block}.tinbopt{display:none}.sbfixed .tinbopt{display:block}.vm2 .ctgstools{float:right;width:48%}.vm2 .ctgsword{float:right;width:50%;max-width:300px}.vm0 .ctgstools,.vm1 .ctgstools{border-bottom:solid 1px rgba(0,0,0,0.1)}.tplext{display:none}.sbfixed .tplext{display:table}.tpltab{xborder-top:solid 1px rgba(0,0,0,0.4);overflow-y:auto}.tpltcl,.tpltcr{box-sizing:border-box;width:50%;position:relative;margin-bottom:2px;xborder-top:solid 1px rgba(0,0,0,0.4);background:#eee;padding-left:10px;padding-right:6px}.tpltcl{float:left;border-right:solid 1px #fff;a:transparent}.tpltcr{float:right;border-left:solid 1px #fff;a:transparent}button.wh100{width:100%;height:100%}.tcrdtabcont{margin:0 -3px}.tcrdtab{width:100%;display:table;border-collapse:separate;border-spacing:3px;table-layout:fixed}.tcrdcell{display:table-cell}.tcrdtab button{width:100%;border-radius:0;padding-left:4px;padding-right:4px;overflow-x:hidden;text-overflow:ellipsis;white-space:nowrap}.tcrdtab button.active{border-color:#000}.tcrdpan{position:relative}.sbclrd .tcrdtabcont{margin:0 -1px}.sbclrd .tcrdtab{border-spacing:1px}.sbclrd .tcrdtab button{background:rgba(0,30,90,0.25);color:#fff;border-color:transparent;padding:6px 4px}.tcrdspecgs .tcrdtab button{background:#f8f8f8;color:#333;padding:5px 4px}.sbclrd .tcrdtab button.active{background:#fff;color:inherit}.butsit,.aleg .butsit{background:#eee;border-bottom-color:rgba(0,0,0,0.5)}.butsit[disabled],.aleg butsit[disabled]{opacity:.8}.tlst .awrap{display:block;padding-top:8px;padding-bottom:8px;white-space:nowrap;border-bottom:solid 1px #e4e4e4}.devtch .tlst .awrap:active{background:rgba(0,0,0,0.15)}.aleg .tlst .awrap{padding-top:8px;padding-bottom:8px}.tlst .tmaxw{display:table;width:100%;max-width:600px;table-layout:fixed}.tlst .tnum{display:table-cell;width:15%}.tlst .tplbl{display:table-cell;width:40%}.tlst .tplone{display:table-cell;width:60%}.tlst .tpllist{font-weight:bold;overflow-x:hidden;text-overflow:ellipsis}.tlst .tplnorm{font-weight:bold;overflow:hidden;text-overflow:ellipsis}.tlst .tplemp{color:#aaa}.vm0 .tlst .tplemp{display:none}.tlst .tplunav{display:none}.tlst .tpar1{display:table-cell;width:25%}.vm0 .tlst .tpar1{display:none}.tlst .tpar0{display:none}.vm0 .tlst .tavail .tpar0{display:block}.tlst .tplrn{display:none}.tlst .tavail .tplnorm .tplrn{display:inline;font-weight:normal;margin-left:.4em}.tlst .tjoin{display:table-cell;width:20%;visibility:hidden}.vm0 .tlst .tjoin,.vm1 .tlst .tjoin{width:15%}.tlst .tjoin button{padding-top:3px;padding-bottom:3px;line-height:.9}.tlst .tavail .tjoin{visibility:inherit}.hvok .tlst .awrap:hover{background:#eaecef;cursor:pointer}.turs .bwrap{border-bottom:solid 1px #e4e4e4;padding-top:1em;padding-bottom:1em}.tulst td{padding:0}.tulst .awrap{display:block;border-bottom:solid 1px #e4e4e4;white-space:nowrap;padding-top:12px;padding-bottom:12px}.vm0 .tulst .awrap{padding-top:10px;padding-bottom:10px}.aleg .tulst .awrap{padding-top:8px;padding-bottom:8px}.tulst .maxw{max-width:600px}.tulst .bl1,.tulst .bl2{display:inline-block;width:50%}.vm0 .tulst .b1{width:50%}.vm0 .tulst .b2{width:50%}.tulst .tid,.tulst .torg,.tulst .tpar,.tulst .tdtup,.tulst .tdtfin{display:inline-block}.tulst .tid{width:50%;max-width:130px}.tulst .tpar{min-width:50%}.vm0 .tulst .tid,.vm0 .tulst .tpar{display:block;width:auto}.tulst .tdtfin{width:50%}.vm0 .tulst .tdtfin,.vm0 .tulst .tpar{display:block;width:auto}.hvok .tulst tr .hv:hover{background:#eaecef;cursor:pointer}.ul{max-width:400px;width:100%;border-collapse:collapse}.ulnm{overflow:hidden;text-overflow:ellipsis}.ul td:first-child{width:70%;white-space:nowrap;padding-left:15px}.uls1{margin:2px 0}.uls1 td{padding-top:4px;padding-bottom:4px}.uls2 td{padding-top:1px;padding-bottom:1px}.uls2 .ulnm{font-weight:bold}.vm2 .aleg .uls2 .ulnm{font-weight:normal}.ulsym{margin:0 5px;font-size:12px}.ulla{color:#aaa;margin-left:4px}.ulla0{display:none}.hvok .ul tr:hover .ulla0{display:inline;color:#fff}button.ulbx{border:0;padding:0 1em;font-weight:bold;background:#e22;color:#fff}.ulnu{padding-right:10px}.ul tr.ulhead{color:#eee}.ul tr.ulhead td{padding-top:0;padding-bottom:0}.ulpan{padding:10px 15px;border-bottom:solid 1px #e4e4e4;margin-bottom:1px}td.m1ac{display:none}.ulm1 td.m1ac{display:table-cell}.ulm1 td.m0ac{display:none}.ulwp{background:#fff;position:absolute;top:0;left:0;right:0;bottom:0}.ulost{overflow-y:scroll;-webkit-overflow-scrolling:touch}.hvok .ul tr:hover{background:#eaecef;cursor:pointer}';
    "undefined" === typeof window.k2img && (window.k2img = {});
    window.k2img["000.png"] = "data:image/png;base64,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";
    window.k2img["001.png"] = "data:image/png;base64,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";
    window.k2img["002.png"] = "data:image/png;base64,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";
    window.k2img["003.png"] = "data:image/png;base64,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";
    window.k2img["004.png"] = "data:image/png;base64,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";
    window.k2img["005.png"] = "data:image/png;base64,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";
    window.k2img["006.png"] = "data:image/png;base64,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";
    window.k2img["010.png"] = "data:image/png;base64,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";
    window.k2img["011.png"] = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABaCAMAAAB5TAO7AAAABGdBTUEAALGPC/xhBQAAAMBQTFRFAAAAAAAAAAAAAAAAAAAAAAAARCIRRCIRRCIRRCIRIhERRCIRRCIRMRkLHREJQCgXFAMBKxQJMxERRCgXUB8ASigXRCIRRCIRVTMiJRQGMxURNyYVVTMiTC4iRDMZMyIRZkREmZmZRDMRVTMzd1VVu6qqVVVEU0QjRCIiVUREZmZViHdmd2Zmd3dmMyIiRDMzZlVViIh3mYiIVTMimZmIiHd3qpmZmYh3ZlVEd2ZVRCIRqqqZVUQz////MyIRRDMimryIdQAAACB0Uk5TAEQRIjNV7lXdZogRzElvnVV4mXcAgTMizCWMw93uZmZCKcEzAAAEK0lEQVRo3u2abVfaMBTHLS0pDxUKCmPonJtuoODGrLXpNWu+/7dakj4Ao0nTNr7YOfu/EWju/fG/Nzct53h29l//tIZzt9+/uOj33fnwffKPrwGCW3z/FN/j2wDgeuYaJQ1n10Di6Mvbgb5EMWEgQ5xLdwE4eitVhGHhGkDcBHfhm0Ih+XRz2Y7RR7fRW4Uigtq4+bAIDhDPmxiTFWVaERxvnvdXfgWLD00ZM4jzNK8hFukPtcLha349hlmzLbW4e8kLsqPlCh5zPy93iwYbbY7uX1NtCFWIbLJlGM3rMlzIgiMlQmCi7MtAzf73afgi9Eg19JiuDdf9Wj4yRkiolki2nNbw4q7DZ66YaisWAeFamzKHjQjBtIawCHkCze4PUfyLqxaDUURQjLR28qWXLn+gtAkFL7TmPAi5MK0tLAKDC43zCrZ86Y420I5HbqH6HOvhJ6aYNlLMY7FXOYXBlq3brppBiAgO3KqdtdsyyWcw/saEFRSmHbqsMBIzKRoS/WZSFHPH41c3yu2LMF9EmkMIj79VWrmhj0yq3VsFoZhnUJ5hHtkxrdpAVjwD8VQzgtkK5RhWQqhIoZiVWYCZSDsI4TkUY98TC2g7CP3Ov2hPOiTwnYm0hRCW5AGG0vvhA1PQFhLwLJ9k+2tMCRNtC0mzjKUtOYUE5FgvHLL560NyCrmTNQX41Z/HAZvfGno9hRAkm5KSajWBCIqk8+foJ5MJCM8jeZ50wSREsr3667L9qNP4ssNO8jR5sda49+ls4RRSfrDMzEJmkt88JiGwLC+XWUh5uSbwg8kEhOeB8sZ/NgqR/CI+RwmTCQjPUw7pfASTEMmxYgsItIckwkn5AWn3kmor2hDJUd8Zm4RIblqdCZiDoHMJxIdqKxoQkQQ+SiAOMgfpdSQQewqmILCUQkrrVX0/Ka2WBHJmOyWbOKi+MQanEM+WPUF2nMGplbqQ1MikI4XYFjIDQbYUwuq1PLFSE5IZseW/TzpOiZW4Uoe3qtSI01FAbGugMZCVgzhRVEtYSTdY0oKReCojwsoImlPSSPCVRnjrrXTsE6jPyALHaiPCiuWli782ZFxZFUZEV3zUrGBpFPKrjHArTtGWpD4igZFTaSQtWANKwbA0GKJg+bToU3LGwHJ0GKJge4pW+7/ufThaRjJKUbFE2wabdG1G2hZrgjQxBQKNajAyiu8VlKQakYDnW1YNRlax7nhPKeccXIZpt5aPPWXkHWKOQUcXwBtZtRkZxeoO0DGmVIAGVhNGQanGMES3IYNRCowHUg6AlyOaMA7MdP1pr4wD0Jv63eY2/jLDOIMrBkJp7fhf6F0NUkILG7kZgREcRhotp8ur8XS6HPnigxRht0LkbgQnAxUSnzltXRxgco5gFa8cbsII4oDjFCjx2igh53BSIf72/f5P4l2Tt9Ef7LSIqW7CJt8AAAAASUVORK5CYII=";
    window.k2img["012.png"] = "data:image/png;base64,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";
    window.k2img["013.png"] = "data:image/png;base64,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";
    window.k2img["014.png"] = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABaCAMAAAB5TAO7AAAABGdBTUEAALGPC/xhBQAAAMBQTFRFAAAAAAAAAAAAAAAAAAAAAAAARCIRQiYOVUM1RygXRCIRRCQSRCIRPSUURCIRIhEREQQAThgJNhcUKhMIHREEIhEMRCIRRCIRVTMiMxcRMyIRRDMiEREARCIiMyIR/+7uo5mOVUQi4drWwLu0///uZmZVRCYdV0REd2Zmu6qqd3dmMyIiUzQjRDMzzMy7qZyRZlVVmYiIiIh3iHd3mYh3ZlVEqpmZd2ZVVUQzmZmIRCIR7u7uqqqZMyIR////RDMihI8o8gAAAB90Uk5TAEQRIjNV7g71fFVm3azMiFkSlXdEckQzzItV7iLuZjFQeH8AAASMSURBVGje7Zp9d6I4FMaLYBxE0dJRcWZfxs602lnqWCEmRpHv/60mL0BBCASkZ8+es88/Vci9P5/cmxg8vbv7X/9pDRZDyzJNyxouBh+Sfz52CfHWEG0QgmuPkE/mvFPSwHQJRsHhnNG3AGHimh1x7ocugcG5VAEk7rADhOVi/1whH3+17m9jWAAH5xoFGNziZuF6GcSbjyBeRVQrDJH/9n5n77mLtgyToCTNwYc8fVYr6KetgIjZrqVcfEwmZBeVy3tK/Bw/uS0abQF2B65fPo4qhH0x7ABB4ymbkzg4qERwTCBG+mTesKsi/8j1FCnoSYz1n60mjGHM8HGkJBwPjxr08vDZf2NCkbIQD/CflWdsQQQDRg0EechPolj9AUB7pkYMSuFBCCh18r0thq+jqA0Fukrr3POZYNRYkAd6pkpBNmzoLmqhHYvcKJRlBH9SoaiVEIuFdu0q9DZ03GbVDoJ5sDes66zdhkq+Bv2Xl5dzFYVqV9NhloeoKgrycrlcflSVhcV7VrURyAbh9hDM4uHovnLPeqLKdu/+mNcjhVyurh332T5mGSr3MBvvqLJV50nrdMp+X7IM2K5aI5COyC1DJchjbkmyFBVrxfQgVbYingrkxylXFZajovQjPiBXyO0pLzE9V9rmdxf2QUfS3iJruIb51npt2F3MCs2yJgNpb62pvML6CzLaMifZC4FfOMKwLF9l/TWOMFXhk53UWyu2wrI40pJ0B5EWhbCbr7dDXlkeIFslpUaaQ7gVSeXn4JWqHPK9vLu+l0NYHsl5cki6hPxZ3l7Wc3nfM8hjfOA98B3gkH13Ko+SnCbNCkizmnCI+e9BlqRLCFlKHqzkkNM2Fidus+9kkHInf5F/qLroLpaHWM0hWGjF0l5wogqI5In4CwipuqgJywPKHyI+ky4hkm1F5xByOyTkTso3SH0UllppB5Fs9b1xOQRlxQufu4LKIZIvrd6DpCgNv+NF3b9IIBopWAlOKqeV01vBCPlDAjFAAXK8KOlbsSQ9CUR3SEvIr0IDL6WQaWG+WjgRs/VZArnTjUITo6PKqf6IriG2LjtB9owJCWsarLa7hJGHnhSia6AbCNClEDpfyzordZDYiC5/PukZtVZqIOIzAqNXAdG1uqowyN91RmYVs8WtiAaTUhA9x++rGaFdZYRbiddK2Oa3AhFJppVGWOk1sexD0pwRBzrVRrgVzSbtrMQMW6sxwqsyBe0mTESBaZ0RZsXQZnFZwuYI2llGrRExYS0oKUNTYPAJS1aLOiVhTDRDhcEnrCklYTxohpKRmPKQUEJlRBOGKIs2A4qYFAGaMGLK1E4pYT2Cro+ppjVgxDPWH79TyjmZ28TpN/LxTsmauQblbnAbTRkxRetPQB5TKgImWhtGSqnHUES/JYNSUoxNpBxC7ATRhpEx0586ozIOISNn2m9v48oM5UzGFATE3LG/ZDSeCMINNhIzHMM5lDRbOs547DjL2ZRfEAj9JkTihnNiUCp+zbjVRQaTcDgrfWUwE50gMhwjRfHXnRISDiOlYm8/7v8kPjT5LfoN2zJ9Whllk78AAAAASUVORK5CYII=";
    window.k2img["015.png"] = "data:image/png;base64,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";
    window.k2img["016.png"] = "data:image/png;base64,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";
    window.k2img["100.png"] = "data:image/png;base64,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";
    window.k2img["101.png"] = "data:image/png;base64,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";
    window.k2img["102.png"] = "data:image/png;base64,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";
    window.k2img["103.png"] = "data:image/png;base64,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";
    window.k2img["104.png"] = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABaCAMAAAB5TAO7AAAABGdBTUEAALGPC/xhBQAAAMBQTFRFAAAAAAAAAAAAAAAAAAAAAAAAAAAAqgAAAAAArQAA4XxvAAAAIAAAqg4MqgQEpAAAgQAAnQAAYAAAqgAAqgAAUQAAZgAAlgAAgAAAuw8Pvx8b/93M/+7d/6qq3Wdi/7627hEA/+7u1E5M7pqZxzYv/5mI7nd33RERuA0N7kRE7oh37iIizBEA7mZV7ndm7lVV7lVE/93d7jMz/5mZ7kQz7mZm7jMiuwAA7oiIqgAA7hER/8zM/6qZ3REA////7iIR8/gxYQAAABt0Uk5TAHcRIlUzZu5ERPqIi+WfLK+GMCt3n7Xnye70Z2RbDQAABWtJREFUaN7tmGt3qjoQhjeiolWsVryeXa+o1SK13hBv/P9/tTMhIGAGgnV/2Gud90MXRZKHdyYzif769b/+ZdXrRU2rVSo1TSvW638DoFVURdVHo69ebzQa6R1FrWhPBdUrSkd/GZ9DGvd0VanUn+VB/RhFAD5o9KE+wU9dU/TFOUZTXfkh5pUgDucEHX6GKap6OE6HhWkul6a5CJMnXbX+cLrVl9tEG3O1dwLar8zN7dMXtfYQpq7q7z5ht3U42u58zlh/xExR6flBWjmoVn7gvpRiWob24a2pSQyCYibswcWHlo5R+2BvuPlyEvXFgnZIR9E67PUOe0dAe/ZG446WhsFW7tIR1DI1pejFauUIa+VFTDD7dcXN+WbrpNDWTcxCqYvVR+8Bhk/pCdVLTd+AxikZhDKmA/WKSELGoMPcSa35gY5UE9PyqvQOoFt5bL5d9eFl2fU32Oyz682tYOjQ5IBpXfqgeXu/96urE6whdn0FyIld3yCOSQd3i0kr62VBNA3U4CYJMglU5RRGmwkrTNOnoGBCJkmQQzAtdLiuxRvZmUShQj+wuYaTycQL3fWd/DNk14tQ6cP4XawVrbsDrYPDptcEmcGn13SCbpwVZQSPhBuvmQTZhVsyzDBS44p9CQoZcXZJkHAXXdMpYlZxRf8CRRpsEiTSRmGGVUzZK3N4ItJPVkmQSG/YwhxzBa/2FegzsiP14xnvkebySSdB15fWnRPdN0aTLtbvU0TfdGGb940SZsF2r0yluyXibLhbqLt19O6a3OyvOZsxzNLFkmKvQdxd/ff1end/T9xxd3w6DZKUNxfC7eIkYPc3r9ff3IddyCuS9z2IOw56152uPDJYASEQrfMJ4g7Dlhb3leg0HX6/r3UwhrPFIPw9mkI05NSIH3gECzEggw+pGEci/tlNrG95gnmMGhdSNWZE3FGmWAf2BPMYFcTJhYhrZYFBFlwjMA8GsVDIBIMccAgSLhxyxiAbFGIhqwuH9EVbcADCd1JT0kP6KIT/5S7zZsOHA96w/zDIiff0gEL4baVAIRekP/LFbcN0GjvDheQUFIJui0MUovAhcvuCJGWPb754mch8SANbXmQPHJ44GnLbMIXYNcRJ2ULiNUeWUZ/bId2UvPF3Rpkl5d7KDimId17zOropkZGDRMGt+QGvqWywRjDhLmCrmkEgcoO3iN1zF5YTEsg9N1ooJMuJ13KYdIIcLjnRymEnSJnF6yJUIkixXNxoyRgkk5PsqJW9ACS4jl0jdi6DQuQss3IUP25HdnrPCAoh8WJWZuLfTsI7/cw1kpXxL0GZXL4ZCdhCBDINB8uq4tECK1mpZYQoGxHIJMQwWnFGqBVWK5fITwVCX1HckXYj1gi10nZzz+r+fBLQOVDrF6tdkON/kSBWvIDN0v5+M2PByicYgQWWL7OAzR5hXOxyTk76lYjUiuSlZZCGMfASkpWTjLgB8yj8owt6QKGM5GCxgJWqKSk+o50vyAIMErBCXmp7lIEA5jjwGFXCEDHipsWrfBEzno2L3ZSyggxIS1YieTGEMD7CsBtSNifKYJRyy7r4GIRz9BEXq1VOxaCUvFRu38yQqrnjHGe3Tw27LeXTMRhFagTMuKvA1yD0gdVqEIacjsHWmFRq2mEMVxbJuCS8rsIUSIxESsZKRJQlko4HGDRkYEYqN1s4x7JbgMgXcg8hmBnAlMgSsC0jCjAs224TAmRcfpThZgYwJcJpthRCsizDMMhfMr9CPZQIovATBFAYBvyUSuVGs9kmajab5VKJ3qWInzFcTC7AYZI8Qu4JCC83wMm7JDZ/Hgg5+TkEZoeCCoWsK3IFgMwTET4oI3vK/AXAv6A/7H53zleWCe4AAAAASUVORK5CYII=";
    window.k2img["105.png"] = "data:image/png;base64,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";
    window.k2img["106.png"] = "data:image/png;base64,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";
    window.k2img["110.png"] = "data:image/png;base64,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";
    window.k2img["111.png"] = "data:image/png;base64,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";
    window.k2img["112.png"] = "data:image/png;base64,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";
    window.k2img["113.png"] = "data:image/png;base64,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";
    window.k2img["114.png"] = "data:image/png;base64,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";
    window.k2img["115.png"] = "data:image/png;base64,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";
    window.k2img["116.png"] = "data:image/png;base64,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";

    function mf() {}
    mf.prototype = new Ac;
    mf.prototype.constructor = mf;
    var nf = ["#f11", "#666"];
    e = mf.prototype;
    e.jf = "#e8b060";
    e.rc = 0;
    e.ha = function(a) {
        Bc.prototype.ha.call(this, a, 1);
        this.la = 9;
        this.H = this.fa = 10;
        this.zd = !0;
        this.wc = 32768;
        this.u = [];
        for (var b = 0; b < this.fa; b++) this.u.push(Array(this.la));
        for (b = 0; 32 > b; b++) this.M.push(new $c(this.u));
        Tc(this, 1, {
            borderRadius: "3px",
            border: "solid 2px rgba(0,0,0,0.7)"
        });
        Tc(this, 2, {
            borderRadius: "3px",
            border: "solid 2px rgba(0,0,0,0.7)"
        });
        var c = [{
            borderRadius: "3px",
            border: "solid 2px rgba(0,24,0,0.8)"
        }, {
            background: "#000",
            opacity: "0.25"
        }];
        for (b = 0; 2 > b; b++) Tc(this, 3 + b, c[1], !0);
        this.Wc = !0;
        this.Lb = h(this.b,
            "div", {
                className: "snum fb",
                style: {
                    color: "#926b36",
                    position: "absolute",
                    ig: 2
                }
            });
        for (b = 0; 2 > b; b++)
            for (c = 0; 2 > c; c++)
                for (var d = 0; 7 > d; d++) this.Mf(b + "" + c + "" + d + ".png");
        this.rc = a.app.rd() ? 1 : 0;
        this.reset()
    };
    e.Ae = function(a) {
        return (1 + (this.Ob ? a : this.la - 1 - a)).toString()
    };
    e.reset = function() {
        var a, b;
        for (a = 0; a < this.M.length; a++) this.M[a].setPosition(-1, -1);
        for (a = 0; a < this.fa; a++)
            for (b = 0; b < this.la; b++) this.u[a][b] = null;
        if (!this.table.qc) {
            var c = 0;
            for (a = 0; a < this.la; a += 2) this.M[c++].setParameters(0, 0, a, this.fa - 4), this.M[c++].setParameters(1, 0, a, 3);
            this.M[c++].setParameters(0, 3, 1, this.fa - 3);
            this.M[c++].setParameters(0, 3, this.la - 2, this.fa - 3);
            this.M[c++].setParameters(1, 3, 1, 2);
            this.M[c++].setParameters(1, 3, this.la - 2, 2);
            var d = [4, 2, 5, 1, 6, 1, 5, 2, 4];
            for (a = 0; 2 > a; a++)
                for (b = 0; b < d.length; b++) this.M[c++].setParameters(a ?
                    1 : 0, d[b], b, a ? 0 : this.fa - 1)
        }
    };
    e.Hd = function() {
        function a(a, b) {
            0 < a && (S.moveTo(l + (a + .5) * k - p, n + (b + .5) * m - t), S.lineTo(l + (a + .5) * k - p, n + (b + .5) * m - p), S.lineTo(l + (a + .5) * k - t, n + (b + .5) * m - p), S.moveTo(l + (a + .5) * k - t, n + (b + .5) * m + p), S.lineTo(l + (a + .5) * k - p, n + (b + .5) * m + p), S.lineTo(l + (a + .5) * k - p, n + (b + .5) * m + t));
            a < c - 1 && (S.moveTo(l + (a + .5) * k + p, n + (b + .5) * m - t), S.lineTo(l + (a + .5) * k + p, n + (b + .5) * m - p), S.lineTo(l + (a + .5) * k + t, n + (b + .5) * m - p), S.moveTo(l + (a + .5) * k + t, n + (b + .5) * m + p), S.lineTo(l + (a + .5) * k + p, n + (b + .5) * m + p), S.lineTo(l + (a + .5) * k + p, n + (b + .5) * m + t))
        }
        var b = this;
        S = this.oc;
        var c = this.la,
            d = this.fa,
            f = Math.round(this.Va / (d + .8)),
            g = Math.round(this.Wa / (c + .4));
        9 * g / 10 < f ? (this.Ka = g, this.Ja = 9 * g / 10) : (this.Ka = 10 * f / 9, this.Ja = f);
        this.Nb = Math.floor((this.Wa - c * this.Ka) / 2);
        this.ib = Math.floor((this.Va - d * this.Ja) / 2);
        f = this.Ga;
        var k = this.Ka * f,
            m = this.Ja * f,
            l = this.Nb * f,
            n = this.ib * f;
        Zc(this);
        S.strokeStyle = "rgba(64,32,0,0.5)";
        S.lineWidth = k / 32;
        S.beginPath();
        for (f = 0; f < c; f++) g = 0 == f || f == c - 1, S.moveTo(l + (f + .5) * k, n + .5 * m), S.lineTo(l + (f + .5) * k, n + ((g ? d : 5) - .5) * m), g || (S.moveTo(l + (f + .5) * k, n + 5.5 * m), S.lineTo(l +
            (f + .5) * k, n + (d - .5) * m));
        for (f = 0; f < d; f++) S.moveTo(l + .5 * k, n + (f + .5) * m), S.lineTo(l + (c - .5) * k, n + (f + .5) * m);
        S.moveTo(l + 3.5 * k, n + .5 * m);
        S.lineTo(l + 5.5 * k, n + 2.5 * m);
        S.moveTo(l + 5.5 * k, n + .5 * m);
        S.lineTo(l + 3.5 * k, n + 2.5 * m);
        S.moveTo(l + 3.5 * k, n + (d - .5) * m);
        S.lineTo(l + 5.5 * k, n + (d - 2 - .5) * m);
        S.moveTo(l + 5.5 * k, n + (d - .5) * m);
        S.lineTo(l + 3.5 * k, n + (d - 2 - .5) * m);
        S.stroke();
        d = S.lineWidth = k / 40;
        var p = 2.5 * d,
            t = p + 3.05 * d;
        S.beginPath();
        S.fillStyle = "#fff";
        [
            [0, 3],
            [1, 2],
            [2, 3],
            [4, 3],
            [6, 3],
            [7, 2],
            [8, 3]
        ].forEach(function(c) {
            var d = c[0];
            c = c[1];
            a(d, c);
            a(d, b.fa -
                1 - c)
        }, this);
        S.stroke();
        u(this.Lb, {
            left: Math.round(this.Wa / 2 - 5) + "px",
            top: Math.floor(this.Va - this.ib + (this.ib - 16) / 2 - 4) + "px"
        });
        Ac.prototype.Hd.call(this)
    };
    e.sd = function(a) {
        return (this.rc ? 14 : 0) + 7 * a.color + a.type % 7
    };
    window.k2start = function() {
        new Pa({
            Qf: of ,
            Tf: 1728,
            Gf: 2,
            alt: "_xq",
            af: !0
        })
    };
    window.k2play = function() {
        new vc
    };

    function of () {} of .prototype = new Md; of .prototype.constructor = of ; of .prototype.ha = function(a) {
        Md.prototype.ha.call(this, a, mf, {
            ud: !0,
            Ee: !0,
            Je: !0,
            cc: "WXF",
            Kd: nf
        })
    }; of .prototype.Be = function(a, b, c) {
        if (5 == a[b]) {
            var d = a[b + 1];
            if (b + 2 + d > c) return b;
            this.u.Na = a.slice(b + 2, b + 2 + d);
            return b + 2 + d
        }
        return b
    }; of .prototype.Sd = function(a) {
        Md.prototype.Sd.call(this, a);
        var b = this;
        h(a, "div", {
            className: "mbsp"
        }, [h("input", {
            type: "checkbox",
            checked: this.app.rd(),
            onchange: function() {
                var a = this.checked,
                    d = b.u;
                d.rc != a && (d.rc = a, Q(d));
                d = b.app;
                null != d.Fb && d.Db != a && (d.Db = a, Ga(d))
            }
        }), this.g("m_sy")])
    };

    function wc() {
        xc.call(this, {
            nf: mf
        })
    }
    wc.prototype = Object.create(xc.prototype);
    wc.prototype.constructor = wc;
    wc.prototype.Te = function(a) {
        function b(a, b, c, d) {
            a = b + (a << 2);
            for (b = 0; b < k; b++)
                if (l[b][c] == a) {
                    if (0 == d) return b;
                    d--
                } return -1
        }

        function c(a, b, c) {
            for (var d = 0, f = 0; f < k; f++) l[f][c] == b + (a << 2) && d++;
            return d
        }

        function d(a, b) {
            for (var d = 0; d < g; d++)
                if (1 < c(a, b, d)) return d;
            return -1
        }
        var f = ["PAHCREK", "pahcrek"],
            g = this.u.la,
            k = this.u.fa,
            m = this.u.Ub,
            l = [],
            n = [],
            p = [],
            t, r, B = 1;
        a = a.split(" ");
        for (t = 0; t < k; t++)
            for (l[t] = Array(g), r = 0; r < g; r++) l[t][r] = -1;
        for (t = 0; t < g; t += 2) l[k - 4][t] = 0, l[3][t] = 1;
        l[k - 3][1] = 12;
        l[k - 3][g - 2] = 12;
        l[2][1] = 13;
        l[2][g -
        2
            ] = 13;
        for (t = 0; 2 > t; t++) {
            r = 0 == t ? k - 1 : 0;
            var x = 0 == t ? 0 : 1;
            l[r][0] = l[r][8] = x + 16;
            l[r][1] = l[r][7] = x + 8;
            l[r][2] = l[r][6] = x + 20;
            l[r][3] = l[r][5] = x + 4;
            l[r][4] = x + 24
        }
        for (; a.length;)
            if (r = a.shift(), 0 != r.length && "." != r.charAt(r.length - 1)) {
                if (4 != r.length) break;
                var H = r.charAt(0),
                    A = r.charAt(1);
                t = r.charAt(2);
                var G = r.charAt(3);
                p.push(r);
                B = 1 - B;
                r = f[B].indexOf(H); - 1 == r && (r = f[1 - B].indexOf(H));
                x = A.charCodeAt(0) - 49;
                var K = G.charCodeAt(0) - 49,
                    C = -1,
                    D = -1;
                C = 0;
                0 == B && (x = g - 1 - x, K = g - 1 - K);
                if (0 <= r) H = d(r, B), "+" == A || "-" == A ? (-1 != H && (x = H), D = c(r, B,
                    x), C = "+" == A ? D - 1 : 0, 0 == B && (C = D - 1 - C)) : 0 == r && (C = c(r, B, x) >> 1);
                else if ("+" == H || "-" == H) {
                    r = 0;
                    x = d(r, B);
                    if (-1 == x) break;
                    D = c(r, B, x);
                    "+" == H && (C = D - 1);
                    0 == B && (C = D - 1 - C)
                } else r = 0, x = H.charCodeAt(0) - 49, 0 == B && (x = g - 1 - x), "+" == A || "-" == A ? (D = c(r, B, x), C = "+" == A ? D - 1 : 0, 0 == B && (C = D - 1 - C)) : C = 1;
                C = b(r, B, x, C);
                if (-1 == C) break;
                "+" == t || "-" == t ? (A = G.charCodeAt(0) - 48, "-" == t && (A = -A), 0 == B && (A = -A), 0 == r || 3 == r || 4 == r || 6 == r ? (K = x, D = C + A) : (t = "+" == t ? 1 : -1, 0 == B && (t = -t), 2 == r ? t *= 1 == Math.abs(K - x) ? 2 : 1 : 5 == r && (t *= 2), D = C + t)) : D = C;
                0 < n.length && n.push(m); - 1 != l[D][K] &&
                n.push(-1 - (K + 10 * D));
                l[D][K] = l[C][x];
                l[C][x] = -1;
                n.push(x + 10 * (C + 10 * (K + 10 * D)))
            } n.push(m);
        return {
            K: n,
            O: p
        }
    };
}).call(this);