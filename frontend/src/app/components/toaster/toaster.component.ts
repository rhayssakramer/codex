import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToasterService, Toast } from '../../services/toaster.service';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-toaster',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="toaster-container">
      <div
        *ngFor="let toast of toasts"
        [class]="'toast toast-' + toast.type"
        [@toastAnimation]
      >
        <div class="toast-content">
          <span class="toast-icon">
            <i *ngIf="toast.type === 'success'" class="fas fa-check-circle"></i>
            <i *ngIf="toast.type === 'error'" class="fas fa-exclamation-circle"></i>
            <i *ngIf="toast.type === 'warning'" class="fas fa-exclamation-triangle"></i>
            <i *ngIf="toast.type === 'info'" class="fas fa-info-circle"></i>
          </span>
          <span class="toast-message">{{ toast.message }}</span>
        </div>
        <button class="toast-close" (click)="toasterService.remove(toast.id)">✕</button>
      </div>
    </div>
  `,
  styles: [`
    .toaster-container {
      position: fixed;
      top: 20px;
      right: 20px;
      z-index: 9999;
      display: flex;
      flex-direction: column;
      gap: 10px;
      max-width: 400px;
    }

    .toast {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 1rem 1.5rem;
      border-radius: 8px;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
      animation: slideIn 0.3s ease;
      min-height: 60px;
    }

    .toast-content {
      display: flex;
      align-items: center;
      gap: 1rem;
      flex: 1;
    }

    .toast-icon {
      font-size: 1.3rem;
      flex-shrink: 0;
    }

    .toast-message {
      font-size: 0.95rem;
      font-weight: 500;
    }

    .toast-close {
      background: none;
      border: none;
      font-size: 1.2rem;
      cursor: pointer;
      padding: 0;
      margin-left: 1rem;
      opacity: 0.7;
      transition: opacity 0.2s ease;
      flex-shrink: 0;
    }

    .toast-close:hover {
      opacity: 1;
    }

    .toast-success {
      background: #d4edda;
      color: #155724;
      border: 1px solid #c3e6cb;
    }

    .toast-success .toast-icon {
      color: #28a745;
    }

    .toast-error {
      background: #f8d7da;
      color: #721c24;
      border: 1px solid #f5c6cb;
    }

    .toast-error .toast-icon {
      color: #dc3545;
    }

    .toast-warning {
      background: #fff3cd;
      color: #856404;
      border: 1px solid #ffeeba;
    }

    .toast-warning .toast-icon {
      color: #ffc107;
    }

    .toast-info {
      background: #d1ecf1;
      color: #0c5460;
      border: 1px solid #bee5eb;
    }

    .toast-info .toast-icon {
      color: #17a2b8;
    }

    @keyframes slideIn {
      from {
        transform: translateX(400px);
        opacity: 0;
      }
      to {
        transform: translateX(0);
        opacity: 1;
      }
    }

    @media (max-width: 480px) {
      .toaster-container {
        left: 10px;
        right: 10px;
        max-width: none;
      }

      .toast {
        padding: 0.8rem 1rem;
      }

      .toast-message {
        font-size: 0.9rem;
      }
    }
  `],
  animations: [
    trigger('toastAnimation', [
      transition(':enter', [
        style({ transform: 'translateX(400px)', opacity: 0 }),
        animate('300ms ease', style({ transform: 'translateX(0)', opacity: 1 }))
      ]),
      transition(':leave', [
        animate('300ms ease', style({ transform: 'translateX(400px)', opacity: 0 }))
      ])
    ])
  ]
})
export class ToasterComponent implements OnInit {
  toasts: Toast[] = [];

  constructor(public toasterService: ToasterService) {}

  ngOnInit(): void {
    this.toasterService.toasts$.subscribe(toasts => {
      this.toasts = toasts;
    });
  }
}
