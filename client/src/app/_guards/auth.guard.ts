import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const accoutService = inject(AccountService);
  const taster = inject(ToastrService);

  if (accoutService.currentUser()) {
    return true;
  }
  else {
    taster.error('You shall not pass!');
    return false;
  }
};
