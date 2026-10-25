
import { test, expect } from '@playwright/test';
import { LoginPage } from '../Pages/Login_page';

test.describe('Login functionality', () => {
  let loginPage: LoginPage;

  test.beforeEach(async ({ page }) => {
    loginPage = new LoginPage(page);
    await loginPage.open();
  });

  test('should log in with valid credentials', async ({ page }) => {
    await loginPage.login(
      'standard_user',
      'secret_sauce'
    );

    await expect(page).toHaveURL(/inventory\.html/);

    await expect(
      page.getByText('Products', { exact: true })
    ).toBeVisible();
  });

  test('should show an error for invalid credentials', async () => {
    await loginPage.login(
      'wrong_user',
      'wrong_password'
    );

    await expect(
      loginPage['errorMessage']
    ).toBeVisible();

    await expect(
      loginPage['errorMessage']
    ).toContainText(
      'Username and password do not match'
    );
  });
});
