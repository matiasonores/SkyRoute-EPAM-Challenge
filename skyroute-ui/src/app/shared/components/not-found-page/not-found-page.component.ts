import { Component } from '@angular/core';

@Component({
  selector: 'app-not-found-page',
  standalone: true,
  template: `
    <div class="container">
      <h1>404</h1>
      <p>Page not found</p>
    </div>
  `,
  styles: [`
    .container{
      text-align:center;
      padding:64px;
    }
  `]
})
export class NotFoundPageComponent {
}