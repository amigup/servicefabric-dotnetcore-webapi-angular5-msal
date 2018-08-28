import {Observable} from 'rxjs/Rx'
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Injectable} from "@angular/core";

@Injectable()
export class HttpServiceHelper {

  constructor(private http: HttpClient) {
  }

  public httpGetRequest(url : string) {
    return this.http.get(url)
      .map(response => {
        return response;
      })
      .catch(response => (Observable.throw(response)
      ))
  }

  public httpAuthorizeGetRequest(url : string, token: string) {
    let headers = new HttpHeaders();
    headers = headers.set("Content-Type", "application/json").set("Authorization", token);
    return this.http.get(url, { headers })
      .map(response => {
        return response;
      })
      .catch(response => (Observable.throw(response)
      ))
  }
}
