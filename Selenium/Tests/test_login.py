import pytest

from Page.Login_page import Login

@pytest.mark.parametrize(
    "email,password",
    [
        ("prateek@automation.pm","Playwright@2026")
    ]
)
def test_login(driver, email, password):
    login_page = Login(driver)
    login_page.open_login_page()
    login_page.login(email,password)
    assert "auth/account" not in driver.current_url.lower()