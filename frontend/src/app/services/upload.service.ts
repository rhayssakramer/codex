import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  // Usar /api/uploads que é redirecionado pelo proxy para http://localhost:5000/api/uploads
  private apiUrl = '/api/uploads';

  constructor(private http: HttpClient) {}

  uploadImage(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.apiUrl}/image`, formData);
  }

  deleteImage(filename: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/image/${filename}`);
  }
}
