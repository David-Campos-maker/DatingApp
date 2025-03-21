import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-test-erros',
    imports: [],
    templateUrl: './test-erros.component.html',
    styleUrl: './test-erros.component.scss'
})
export class TestErrosComponent {
  baseUrl = environment.apiUrl;
  private readonly http = inject(HttpClient);

  validationErrors: string[] = [];

  get400Error() {
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe({
      next: response => console.log(response),
      error: error => console.error(error)
    })
  }
  get401Error() {
    this.http.get(this.baseUrl + 'buggy/auth').subscribe({
      next: response => console.log(response),
      error: error => console.error(error)
    })
  }
  get404Error() {
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe({
      next: response => console.log(response),
      error: error => console.error(error)
    })
  }
  get500Error() {
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe({
      next: response => console.log(response),
      error: error => console.error(error)
    })
  }
  get400ValidationError() {
    this.http.post(this.baseUrl + 'account/register', {}).subscribe({
      next: response => console.log(response),
      error: error => {
        console.error(error)
        this.validationErrors = error
      }
    })
  }
}
