var pageURL = window.location.origin;
if (pageURL.includes("localhost") == true) {
    pageURL = window.location.pathname;
    var index = pageURL.indexOf("ke") + 2;
    pageURL = pageURL.substring(0, index);
}

//const instance = axios.create({
//    baseURL: 'https://some-domain.com/api/',
//    timeout: 1000,
//    headers: { 'X-Custom-Header': 'foobar' }
//});

export class HttpRequest {
    constructor(method, url) {
        this.method = method;
        this.url = url;
        this.axios = axios.create({
            baseURL: pageURL,
            timeout: 5000
            //headers: { 'X-Custom-Header': 'foobar' }
        });
    }
}

export const name = 'square';

export function draw(ctx, length, x, y, color) {
    ctx.fillStyle = color;
    ctx.fillRect(x, y, length, length);

    return {
        length: length,
        x: x,
        y: y,
        color: color
    };
}
