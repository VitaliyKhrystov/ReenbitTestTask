import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FileService {

  constructor(private http: HttpClient) { }
  baseApiUrl: string = environment.apiURL;

  upload(file: FormData) {
    return this.http.post(this.baseApiUrl + "files/upload", file)
  }

}
