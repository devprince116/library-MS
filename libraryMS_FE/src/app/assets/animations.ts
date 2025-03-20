
import { trigger, transition, style, animate } from '@angular/animations';

export const fadeInOut = trigger('fadeInOut', [
  transition(':enter', [
    style({ opacity: 0 }),
    animate('300ms ease-in', style({ opacity: 1 })),
  ]),
  transition(':leave', [
    animate('300ms ease-out', style({ opacity: 0 })),
  ]),
]);

export const slideInOut = trigger('slideInOut', [
  transition(':enter', [
    style({ transform: 'translateY(-20px)', opacity: 0 }),
    animate('400ms ease-in', style({ transform: 'translateY(0)', opacity: 1 })),
  ]),
  transition(':leave', [
    animate('400ms ease-out', style({ transform: 'translateY(-20px)', opacity: 0 })),
  ]),
]);